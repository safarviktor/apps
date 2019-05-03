using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OsloBysykkel.DataAccess;
using OsloBysykkel.Models;

namespace OsloBysykkel.DataMigration
{
    public class MigrationWorker
    {
        private readonly StationsRepository _stationsRepository = new StationsRepository();
        private readonly StationAvailabilitiesRepository _stationAvailabilitiesRepository = new StationAvailabilitiesRepository();

        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly string _runId = DateTime.Now.ToString("yyyyMMdd_HHmmss");

        private List<StationAvailabilityAtTime> _availabilitiesToSave;
        private List<int> _existingStationsCache = new List<int>();

        public async Task Execute()
        {
            var files = GetFiles().ToList();
            var total = files.Count;
            var currentFileIndex = 1;

            foreach (var file in files)
            {
                _log.Info($"Processing file {currentFileIndex} of {total}: {file}");
                try
                {
                    var rawData = FileRepository.ReadFile(file);

                    _log.Info($"Saving file {file}");
                    await SaveData(rawData.ToList());
                    
                    ArchiveFile(file);

                    currentFileIndex++;
                }
                catch (Exception e)
                {
                    _log.Error(e.ToString());
                }
            }
        }

        private void ArchiveFile(string file)
        {
            var fi = new FileInfo(file);
            var newFolder = Path.Combine(fi.DirectoryName, "_Archive_/", _runId + "/");
            var newName = Path.Combine(newFolder, fi.Name);
            _log.Info($"Archiving file {file} as {newName}");

            if (!Directory.Exists(newFolder))
            {
                Directory.CreateDirectory(newFolder);
            }

            File.Move(file, newName);
        }

        private async Task SaveData(IList<StationAvailabilityAtTime> rawData)
        {
            _availabilitiesToSave = new List<StationAvailabilityAtTime>();

            _log.Info($"Adding {rawData.Count} availabilities");
            foreach (var rawRow in rawData)
            {
                await AddAvailabilityRow(rawRow);
            }

            await _stationAvailabilitiesRepository.AddStationAvailabilities(_availabilitiesToSave);
        }

        private async Task AddAvailabilityRow(StationAvailabilityAtTime rawRow)
        {
            if (await StationDoesNotExist(rawRow))
            {
                _log.Info($"Station {rawRow.Station.Id} does not exist, creating ...");
                await CreateStation(rawRow);
            }

            _availabilitiesToSave.Add(rawRow);
        }

        private async Task CreateStation(StationAvailabilityAtTime rawRow)
        {
            var fullStation = await _stationsRepository.AddStation(rawRow.Station);
            _existingStationsCache.Add(rawRow.Station.Id);
        }

        private async Task<bool> StationDoesNotExist(StationAvailabilityAtTime rawRow)
        {
            _log.Info($"Checking if station {rawRow.Station.Id} exists");

            if (_existingStationsCache.Contains(rawRow.Station.Id))
            {
                return false;
            }

            var stationExists = await _stationsRepository.StationExists(rawRow.Station.Id);

            if (stationExists)
            {
                _existingStationsCache.Add(rawRow.Station.Id);
            }

            return !stationExists;
        }

        private static IEnumerable<string> GetFiles()
        {
            return Directory.EnumerateFiles(ConfigurationManager.AppSettings["DataDirectory"]);
        }
    }
}
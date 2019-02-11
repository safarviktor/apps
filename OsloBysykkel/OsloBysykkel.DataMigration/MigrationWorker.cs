using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using OsloBysykkel.DataAccess;
using OsloBysykkel.Models;

namespace OsloBysykkel.DataMigration
{
    public class MigrationWorker
    {
        private readonly StationsRepository _stationsRepository = new StationsRepository();
        private readonly StationAvailabilitiesRepository _stationAvailabilitiesRepository = new StationAvailabilitiesRepository();

        public async Task Execute()
        {
            var files = GetFiles();

            foreach (var file in files)
            {
                var rawData = FileRepository.ReadFile(file);
                await SaveData(rawData);
                ArchiveFile(file);
            }
        }

        private void ArchiveFile(string file)
        {
            throw new System.NotImplementedException();
        }

        private async Task SaveData(IEnumerable<StationAvailabilityAtTime> rawData)
        {
            foreach (var rawRow in rawData)
            {
                await SaveAvailabilityRow(rawRow);
            }
        }

        private async Task SaveAvailabilityRow(StationAvailabilityAtTime rawRow)
        {
            if (await StationDoesNotExist(rawRow))
            {
                await CreateStation(rawRow);
            }

            await _stationAvailabilitiesRepository.AddStationAvailability(rawRow);
        }

        private async Task CreateStation(StationAvailabilityAtTime rawRow)
        {
            var fullStation = await _stationsRepository.AddStation(rawRow.Station);
        }

        private async Task<bool> StationDoesNotExist(StationAvailabilityAtTime rawRow)
        {
            return !await _stationsRepository.StationExists(rawRow.Station.Id);
        }

        private static IEnumerable<string> GetFiles()
        {
            return Directory.EnumerateFiles(_directory);
        }
    }
}
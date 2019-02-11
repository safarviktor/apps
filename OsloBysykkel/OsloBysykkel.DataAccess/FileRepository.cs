using System;
using System.Collections.Generic;
using System.IO;
using OsloBysykkel.Models;

namespace OsloBysykkel.DataAccess
{
    public class FileRepository
    {
        private const string _directory = @"C:\Coding\GitHub\apps\OsloBysykkel\Data";

        private List<StationAvailabilityAtTime> _cache = new List<StationAvailabilityAtTime>();

        public FileRepository()
        {
            ReadData();
        }

        public List<StationAvailabilityAtTime> GetStationAvailabilityAtTimes()
        {
            return _cache;
        }

        private void ReadData()
        {
            foreach (var file in Directory.EnumerateFiles(_directory))
            {
                _cache.AddRange(ReadFile(file));
            }
        }

        public static IEnumerable<StationAvailabilityAtTime> ReadFile(string file)
        {
            var lines = File.ReadAllLines(file);
            foreach (var line in lines)
            {
                if (line.StartsWith("Time"))
                    continue;
                yield return GetLine(line);
            }
        }

        private static StationAvailabilityAtTime GetLine(string line)
        {
            var splits = line.Split(';');

            try
            {
                return new StationAvailabilityAtTime()
                {
                    Time = Convert.ToDateTime(splits[0]),
                    Availability = new Availability()
                    {
                        Bikes = Convert.ToInt32(splits[7]),
                        Locks = Convert.ToInt32(splits[6]),
                        UpdatedAt = Convert.ToDateTime(splits[8]),
                        RefreshRate = Convert.ToDecimal(splits[9]),
                    },
                    Station = new Station()
                    {
                        Id = Convert.ToInt32(splits[1]),
                        Title = splits[2],
                        Subtitle = splits[3],
                        In_Service = Convert.ToBoolean(splits[4]),
                        Number_Of_Locks = Convert.ToInt32(splits[5]),
                    }
                };
            }
            catch (Exception e)
            {
                return new StationAvailabilityAtTime()
                {
                    Station = new Station()
                    {
                        Id = 0
                    }
                };
            }
        }
    }
}
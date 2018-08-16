using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OsloBysykkel.Models;
using System.IO;

namespace OsloBysykkel
{
    public static class AvailabilityLogger
    {
        public static void Execute()
        {
            var caller = new ApiCaller();
            var availabilities = caller.GetStationsAvailabilities().GetAwaiter().GetResult();
            var stations = caller.GetStationsDescriptions().GetAwaiter().GetResult();

            var logFile = "OsloBysykkelAvailabilityLog.txt";
            if (!File.Exists(logFile))
            {
                File.WriteAllText(logFile, GetHeaders());
            }

            var logContent = GetLogLines(availabilities, stations);

            File.AppendAllLines(logFile, logContent);
        }

        private static IEnumerable<string> GetLogLines(AvailabilityResult availabilities, StationDescriptionsResult stations)
        {            
            var logTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            foreach (var availability in availabilities.Stations)
            {
                var stationDescription = stations.Stations.FirstOrDefault(x => x.Id == availability.Id);
                var log = $"{logTime};{availability.Id};{stationDescription?.Title};";
                log += $"{stationDescription?.Subtitle};{stationDescription?.In_Service};";
                log += $"{stationDescription?.Number_Of_Locks};{availability.Availability.Locks};{availability.Availability.Bikes};";
                log += $"{availabilities.Updated_At};{availabilities.Refresh_Rate};";
                yield return log;
            }
        }

        private static string GetHeaders()
        {
            return "Time;Id;Title;Subtitle;InService;NumbeOfLocks;ActualLocks;ActualBikes;UpdatedAt;RefreshRate;" + Environment.NewLine;
        }
    }
}

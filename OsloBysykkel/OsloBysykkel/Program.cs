using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Timers;


namespace OsloBysykkel
{
    class Program
    {
        static void Main(string[] args)
        {
            var intervalInSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["IntervalInSeconds"]);
            var timer = new Timer();
            timer.Interval = intervalInSeconds * 1000;
            timer.Elapsed += TimerOnElapsed;
            timer.Enabled = true;

            var stopCandidate = string.Empty;
            while (stopCandidate != "stop")
            {
                stopCandidate = Console.ReadLine();
            }

            timer.Enabled = false;
        }

        private static void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm")} : Checking availability . . .");
                AvailabilityLogger.Execute();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {ex}");
            }

        }
    }
}

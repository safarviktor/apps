using System;
using System.Configuration;
using System.Timers;
using Castle.Windsor;
using OsloBysykkel.Setup;

namespace OsloBysykkel
{
    public class Worker
    {
        private readonly WindsorContainer _container;

        public Worker()
        {
            _container = new WindsorContainer();
            _container.Install(new RepositoriesInstaller());
        }

        public void Execute()
        {
            var intervalInSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["IntervalInSeconds"]);
            var timer = new Timer();
            timer.Interval = intervalInSeconds * 1000;
            timer.Elapsed += TimerOnElapsed;
            timer.Enabled = true;
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
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
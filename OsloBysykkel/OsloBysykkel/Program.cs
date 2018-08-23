using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Timers;
using Castle.Windsor;


namespace OsloBysykkel
{
    class Program
    {
        static void Main(string[] args)
        {
            var worker = new Worker();
            worker.Execute();
            var stopCandidate = string.Empty;
            while (stopCandidate != "stop")
            {
                stopCandidate = Console.ReadLine();
            }
        }
    }
}

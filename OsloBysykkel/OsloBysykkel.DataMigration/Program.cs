using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OsloBysykkel.DataAccess;
using OsloBysykkel.Models;

namespace OsloBysykkel.DataMigration
{
    class Program
    {
        static void Main(string[] args)
        {
            var worker = new MigrationWorker();
            worker.Execute();
        }
    }
}

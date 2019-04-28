namespace OsloBysykkel.DataMigration
{
    class Program
    {
        static void Main(string[] args)
        {
            var worker = new MigrationWorker();
            worker.Execute().GetAwaiter().GetResult();
        }
    }
}

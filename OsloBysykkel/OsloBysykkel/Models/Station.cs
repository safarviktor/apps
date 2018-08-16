using System.Collections.Generic;

namespace OsloBysykkel.Models
{
    public class Station
    {
        public int Id { get; set; }
        public bool In_Service { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public int Number_Of_Locks { get; set; }
        public Location Center { get; set; }
        public Location[] Bounds { get; set; }
    }
}

using System;

namespace OsloBysykkel.Models
{
    public class StationAvailabilityAtTime
    {
        public DateTime Time { get; set; }

        public Availability Availability { get; set; }

        public Station Station { get; set; }
    }
}
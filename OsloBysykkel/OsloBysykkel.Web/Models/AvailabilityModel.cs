using System;

namespace OsloBysykkel.Web.Models
{
    public class AvailabilityModel
    {
        public DateTime AsAt { get; set; }

        public int Bikes { get; set; }

        public int Locks { get; set; }
    }
}
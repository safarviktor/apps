using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsloBysykkel.Models
{
    public class AvailabilityResult
    {
        public StationAvailability[] Stations { get; set; }
        public DateTime Updated_At { get; set; }
        public decimal Refresh_Rate { get; set; }
    }
}

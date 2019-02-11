using System;
using System.Collections.Generic;

namespace OsloBysykkel.Web.Models
{
    public class StationAvailabilityInTimeModel
    {
        public StationModel Station { get; set; }

        public List<AvailabilityModel> Availabilities { get; set; }
    }
}
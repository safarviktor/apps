using System.Collections.Generic;
using OsloBysykkel.DataAccess;
using OsloBysykkel.Models;

namespace OsloBysykkel.Web.App_Data
{
    public class Cache
    {
        public static Cache _instance = null;

        public List<StationAvailabilityAtTime> StationAvailabilities;

        public static Cache Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Cache();
                    var repo = new FileRepository();
                    _instance.StationAvailabilities = repo.GetStationAvailabilityAtTimes();
                }
                
                return _instance;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OsloBysykkel.DataAccess;
using OsloBysykkel.Web.App_Data;
using OsloBysykkel.Web.Models;

namespace OsloBysykkel.Web.Controllers
{
    public class StationAvailabilityController : Controller
    {
        public ActionResult Index()
        {
            var model = new List<StationAvailabilityInTimeModel>();

            foreach (var stationAvailabilities in Cache.Instance.StationAvailabilities.GroupBy(x => x.Station.Id))
            {
                var stationData = stationAvailabilities.First().Station;

                model.Add(new StationAvailabilityInTimeModel()
                {
                    Station = new StationModel()
                    {
                        Id = stationData.Id,
                        Title = stationData.Title,
                        Subtitle = stationData.Subtitle
                    },
                    Availabilities = stationAvailabilities.Where(x => x.Availability != null).Select(x => new AvailabilityModel()
                    {
                        Bikes = x.Availability.Bikes,
                        AsAt = x.Time,
                        Locks = x.Availability.Locks
                    }).ToList()

                });
            }

            return View(model.OrderBy(x => x.Station.Title));
        }

        public ActionResult SingleStationAvailability(int id)
        {
            var availabilities = Cache.Instance.StationAvailabilities.Where(x => x.Station.Id == id);
            var station = availabilities.First().Station;

            var model = new StationAvailabilityInTimeModel()
            {
                Station = new StationModel()
                {
                    Id = station.Id,
                    Title = station.Title,
                    Subtitle = station.Subtitle
                },
                Availabilities = availabilities.Where(x => x.Availability != null).OrderBy(x => x.Time).Select(x => new AvailabilityModel()
                {
                    AsAt = x.Time,
                    Bikes = x.Availability.Bikes,
                    Locks = x.Availability.Locks
                }).ToList()
            };

            return View(model);
        }
    }
}
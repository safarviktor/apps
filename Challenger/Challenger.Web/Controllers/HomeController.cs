using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Challenger.DataAccess;
using Challenger.Models;
using Challenger.Web.Models;

namespace Challenger.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _dataContext = new DataContext();

        public ActionResult Index()
        {
            var challengeOverviews = _dataContext.GetChallengeOverviews();
            return View(new HomeViewModel() { Challenges = challengeOverviews, Name = "Vik" });
        }

        public ActionResult Challenge(int id)
        {
            var model = _dataContext.GetChallengeDetails(id);
            return View(new ChallengeViewModel()
            {
                Name = model.Name,
                CurrentTotal = model.CurrentTotal,
                TargetTotal = model.TargetTotal,
                LastEntry = model.LastEntry.GetValueOrDefault(),
                LastEntryCount = model.LastEntryCount,
                TargetTotalTodo = model.TargetTotalTodo,
                TodayCount = model.TodayCount,
                TodayGoal = model.TodayGoal,
                TodayTodo = model.TodayTodo,
                AddSetViewModel = new AddSetViewModel()
                {
                    ChallengeId = id,
                    Count = 10,
                    Date = DateTime.Now
                },
                SetsByDate = model.SetsByDay.Select(x => new SetByDateViewModel()
                {
                    Date = x.Date,
                    Sets = x.Sets.Select(s => new SetViewModel()
                    {
                        Count = s.Count,
                        Date = s.Date
                    }).ToList()
                }).ToList()
            });
        }

        public ActionResult AddSet(AddSetViewModel model)
        {
            var set = _dataContext.AddNewSet(new TrackSetModel()
            {
                ChallengeId = model.ChallengeId,
                Count = model.Count,
                Date = model.Date
            });

            return RedirectToAction("Challenge", new {Id = model.ChallengeId});
        }
    }
}
using Challenger.DataAccess;
using Challenger.Models;
using Challenger.Web.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Challenger.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ChallengerRepository _dataContext = new ChallengerRepository();

        public async Task<ActionResult> Index()
        {
            var challengeOverviews = await _dataContext.GetChallengeOverviews(1);
            return View(new HomeViewModel() { Challenges = challengeOverviews, Name = "Vik" });
        }

        public async Task<ActionResult> Challenge(int id)
        {
            var model = await _dataContext.GetChallengeDetails(id);
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
                    Count = 30,
                    Date = DateTime.Now
                },
                SetsByDate = model.SetsByDay.Select(x => new SetByDateViewModel()
                {
                    Date = x.Date,
                    Sets = x.Sets.Select(s => new SetViewModel()
                    {
                        Count = s.Repetitions,
                        Date = s.Date
                    }).ToList()
                }).ToList()
            });
        }

        public async Task<ActionResult> AddSet(AddSetViewModel model)
        {
            var set = await _dataContext.AddNewSet(new TrackSetModel()
            {
                ChallengeId = model.ChallengeId,
                Count = model.Count,
                Date = model.Date
            });

            return RedirectToAction("Challenge", new {Id = model.ChallengeId});
        }
    }
}
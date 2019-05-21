using Challenger.DataAccess;
using Challenger.Models;
using Challenger.Web.Models;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Challenger.Common;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;

namespace Challenger.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ChallengerRepository _dataContext = new ChallengerRepository();

        [HttpGet]
        public async Task<ActionResult> Index(AddChallengeViewModel newChallenge = null)
        {
            var challengeOverviews = await _dataContext.GetChallengeOverviews(User.Identity.GetUserId());
            return View(new HomeViewModel()
            {
                NewChallenge = newChallenge ?? new AddChallengeViewModel(),
                Challenges = challengeOverviews,
                Name = "Vik"
            });
        }

        [HttpGet]
        public async Task<ActionResult> Challenge(int id, string message = "")
        {
            var model = await _dataContext.GetChallengeDetails(id, User.Identity.GetUserId());

            if (model == null)
            {
                return RedirectToAction("Index");
            }

            var defaultSetReps = 30;

            return View(new ChallengeViewModel()
            {
                Id = model.Id,
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
                    Count = defaultSetReps > model.TodayTodo ? model.TodayTodo : defaultSetReps,
                    Date = DateTime.Now,
                    Message = message
                },
                SetsByDate = model.SetsByDay.Select(x => new SetByDateViewModel()
                {
                    Date = x.Date,
                    Sets = x.Sets.Select(s => new SetViewModel()
                    {
                        Id = s.Id,
                        Count = s.Repetitions,
                        Date = s.Date
                    }).ToList()
                }).ToList()
            });
        }

        [HttpPost]
        public async Task<ActionResult> AddChallenge(AddChallengeViewModel model)
        {
            if (model.Type == ChallengeType.NA)
            {
                model.Message = "Please select a valid challenge type";
                return RedirectToAction("Index", model);
            }

            if (model.Name.IsNullOrWhiteSpace())
            {
                model.Message = "Please set a challenge name, e.g. 'Squats 2019'";
                return RedirectToAction("Index", model);
            }

            await _dataContext.AddNewChallenge(model.Name, model.Type, User.Identity.GetUserId());

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> AddSet(AddSetViewModel model)
        {
            if (model.Count < 1)
            {
                return RedirectToAction("Challenge", new { Id = model.ChallengeId, message = "Repetitions must be a number greater than 0." });
            }

            if (model.Date.Date > DateTime.Now.Date)
            {
                return RedirectToAction("Challenge", new { Id = model.ChallengeId, message = "Date cannot be greater than today." });
            }

            var challenge = await _dataContext.GetChallengeDetails(model.ChallengeId, User.Identity.GetUserId());
            if (challenge == null)
            {
                return Naughty();
            }

            var set = await _dataContext.AddNewSet(new TrackSetModel()
            {
                ChallengeId = model.ChallengeId,
                Count = model.Count,
                Date = model.Date
            });

            return RedirectToAction("Challenge", new {Id = model.ChallengeId});
        }

        [HttpGet]
        public async Task<ActionResult> DeleteSet(int setId, int challengeId)
        {
            var challenge = await _dataContext.GetChallengeDetails(challengeId, User.Identity.GetUserId());
            if (challenge == null)
            {
                return Naughty();
            }

            var success = await _dataContext.DeleteSet(setId, challengeId);

            if (success)
            {
                return RedirectToAction("Challenge", new { Id = challengeId });
            }

            return Naughty();
        }

        private ActionResult Naughty()
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Naughty!");
        }
    }
}
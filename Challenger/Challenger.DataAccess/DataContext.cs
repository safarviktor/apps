using System;
using System.Collections.Generic;
using System.Linq;
using Challenger.Models;

namespace Challenger.DataAccess
{
    public class DataContext
    {
        public ChallengeOverviewModel AddNewChallenge(string name, ChallengeType type)
        {
            var nextId = Database.Instance.ChallengeOverviews.Any()
                ? Database.Instance.ChallengeOverviews.Max(x => x.Id) + 1
                : 1;

            var model = new ChallengeOverviewModel()
            {
                Id = nextId,
                Name = name,
                Type = type
            };

            Database.Instance.ChallengeOverviews.Add(model);

            return model;
        }

        public ChallengeSetModel AddNewSet(TrackSetModel model)
        {
            var nextId = Database.Instance.ChallengeSets.Any()
                ? Database.Instance.ChallengeSets.Max(x => x.Id) + 1
                : 1;

            var newModel = new ChallengeSetModel()
            {
                ChallengeId = model.ChallengeId,
                Count = model.Count,
                DateTimeCreated = DateTime.Now,
                Date = model.Date == new DateTime() ? DateTime.Now.Date : model.Date,
                Id = nextId
            };

            Database.Instance.ChallengeSets.Add(newModel);

            return newModel;
        }

        public TrackSetModel GetSetForNewTracking(int challengeId)
        {
            var challenge = Database.Instance.ChallengeOverviews.FirstOrDefault(x => x.Id == challengeId);

            var model = new TrackSetModel()
            {
                ChallengeId = challengeId,
                ChallengeName = challenge?.Name,
                Date = DateTime.Today.Date
            };

            return model;
        }

        public IEnumerable<ChallengeOverviewModel> GetChallengeOverviews()
        {
            var today = DateTime.Today.Date;
            return Database.Instance.ChallengeOverviews.Select(c => new ChallengeOverviewModel()
            {
                Id = c.Id,
                Name = c.Name,
                CurrentTotal = Database.Instance.ChallengeSets.Where(set => set.ChallengeId == c.Id)?.Sum(set => set.Count) ?? 0,
                LastEntry = Database.Instance.ChallengeSets.Where(set => set.ChallengeId == c.Id).OrderBy(x => x.DateTimeCreated).LastOrDefault()?.DateTimeCreated ?? null,
                LastEntryCount = Database.Instance.ChallengeSets.Where(set => set.ChallengeId == c.Id).OrderBy(x => x.DateTimeCreated).LastOrDefault()?.Count ?? 0,
                TodayCount = Database.Instance.ChallengeSets.Where(set => set.ChallengeId == c.Id && set.Date == today)?.Sum(x => x.Count) ?? 0,
                Type = c.Type
            });
        }

        public ChallengeDetailModel GetChallengeDetails(int id)
        {
            var challenge = Database.Instance.ChallengeOverviews.FirstOrDefault(x => x.Id == id);
            var sets = Database.Instance.ChallengeSets.Where(x => x.ChallengeId == id);

            var model = new ChallengeDetailModel()
            {
                CurrentTotal = sets.Sum(x => x.Count),
                Id = id,
                Name = challenge.Name,
                Type = challenge.Type,
                Sets = sets.Select(x => new ChallengeSetModel()
                        {
                            DateTimeCreated = x.DateTimeCreated,
                            Count = x.Count,
                            Id = x.Id,
                            Date = x.Date
                        })
                        .OrderByDescending(x => x.DateTimeCreated)
                        .ToList(),
                LastEntryCount = sets.OrderByDescending(x => x.DateTimeCreated).FirstOrDefault()?.Count ?? 0,
                LastEntry = sets.OrderByDescending(x => x.DateTimeCreated).FirstOrDefault()?.DateTimeCreated,
                TodayCount = sets.Where(x => x.Date == DateTime.Today.Date)?.Sum(x => x.Count) ?? 0,
            };

            return model; 
        }
    }
}
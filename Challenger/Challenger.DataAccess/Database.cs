using System.Collections.Generic;
using Challenger.Models;

namespace Challenger.DataAccess
{
    public sealed class Database
    {
        private static readonly Database instance = new Database();

        private Database()
        {
            ChallengeSets = new List<ChallengeSetModel>();
            //ChallengeOverviews = new List<ChallengeOverviewModel>();
            ChallengeOverviews = new List<ChallengeOverviewModel>()
            {
                new ChallengeOverviewModel()
                {
                    Name = "Squats 2018",
                    CurrentTotal = 0,
                    Id = 1,
                    Type = ChallengeType.Squats
                },
                new ChallengeOverviewModel()
                {
                    Name = "Pushups 2018",
                    CurrentTotal = 0,
                    Id = 2,
                    Type = ChallengeType.Pushups
                }
            };

            
        }

        public static Database Instance
        {
            get
            {
                return instance;
            }
        }

        public IList<ChallengeOverviewModel> ChallengeOverviews { get; set; }

        public IList<ChallengeSetModel> ChallengeSets { get; set; }
    }
}
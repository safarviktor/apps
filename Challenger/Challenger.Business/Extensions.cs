using System;
using Challenger.Models;

namespace Challenger.Business
{
    public static class Extensions
    {
        public static void UpdateTodayGoal(this ChallengeOverviewModel c)
        {
            if (c.Type == ChallengeType.AddOneMoreEachDay)
            {
                c.TodayGoal = DateTime.Now.DayOfYear;
                c.TodayTodo = DateTime.Now.DayOfYear - c.TodayCount;
            }
        }
    }
}
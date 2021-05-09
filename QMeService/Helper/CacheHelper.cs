using System.Collections.Generic;

namespace Bumbleberry.QMeService.Helper
{
    public static class CacheHelper
    {
        private static IEnumerable<Models.Queue> ActivityQueue = new List<Models.Queue>();
        private static IEnumerable<Models.Activity> Activities = new List<Models.Activity>();

        public static IEnumerable<Models.Queue> GetActivityQueue()
        {
            //check if cache is expired
            if (ActivityQueue == null)
                ActivityQueue = new List<Models.Queue>();
            return ActivityQueue;
        }

        public static void SetActivityQueue(IEnumerable<Models.Queue> activityQueue)
        {
            ActivityQueue = activityQueue;
        }

        public static IEnumerable<Models.Activity> GetActivities()
        {
            if (Activities == null)
                Activities = new List<Models.Activity>();
            return Activities;
        }

        public static void SetActivities(IEnumerable<Models.Activity> activities)
        {
            Activities = activities;
        }
    }
}

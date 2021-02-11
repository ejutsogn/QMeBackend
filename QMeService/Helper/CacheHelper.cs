using Bumbleberry.QMeService.Models;
using System.Collections.Generic;

namespace Bumbleberry.QMeService.Helper
{
    public static class CacheHelper
    {
        private static IEnumerable<Models.Queue> ActivityQueue = new List<Models.Queue>();

        public static IEnumerable<Models.Queue> GetActivityQueue()
        {
            //check if cache is expired
            return ActivityQueue;
        }

        public static void SetActivityQueue(IEnumerable<Models.Queue> activityQueue)
        {
            ActivityQueue = activityQueue;
        }

        //private static List<QueueInfoLight> queueInfoLightList = new List<QueueInfoLight>();

        //public static IEnumerable<QueueInfoLight> GetQueueInfoLightList()
        //{
        //    return queueInfoLightList;
        //}
    }
}

using Bumbleberry.QMeService.Data;
using Bumbleberry.QMeService.Models;
using System.Collections.Generic;
using System.Linq;

namespace Bumbleberry.QMeService.Biz
{
    public class QueueBiz
    {
        IQueueData _queueData;
        ActivityData _activityData;

        public QueueBiz()
        {
            _queueData = new QueueData();
            _activityData = new ActivityData();
            AutomaticPopulateQueue();
        }

        public QueueBiz(IQueueData queueData)
        {
            _queueData = queueData;
            _activityData = new ActivityData();
        }

        /// <summary>
        /// Add person
        /// * Person removed from queue after 2 minutes
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public QueueInfo AddInQueue(string countryId, string companyId, string userId, string activityId)
        {
            _queueData.Remove(activityId);
            _queueData.Add(countryId, companyId, userId, activityId);
            var queueInfo = GetActivityQueueInfo(countryId, companyId, userId, activityId);
            
            return queueInfo;
        }

        /// <summary>
        /// Person removed from queue after 2 minutes
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public QueueInfo UpdateQueue(string countryId, string companyId, string userId, string activityId)
        {
            _queueData.Update(activityId);
            var queueInfo = GetActivityQueueInfo(countryId, companyId, userId, activityId);

            return queueInfo;
        }

        public QueueInfo RemoveFromQueue(string countryId, string companyId, string userId, string activityId)
        {
            _queueData.RemovePerson(countryId, companyId, activityId, userId);
            var queueInfo = GetActivityQueueInfo(countryId, companyId, userId, activityId);
            return queueInfo;
        }

        public QueueInfo GetActivityQueueInfo(string countryId, string companyId, string userId, string activityId)
        {
            var queueInfos = GetActivityQueueInfos(countryId, companyId, userId);
            var queueInfo = queueInfos.FirstOrDefault(x => x.ActitityId == activityId);
            return queueInfo;
        }

        protected IEnumerable<QueueInfo> GetActivityQueueInfos(string countryId, string companyId, string userId)
        {
            var queueInfos = new List<QueueInfo>(); 
            var activityQueues = _queueData.GetActivityQueues();

            var activityQueuesGroupedByActivity = activityQueues
                                    .GroupBy(x => x.ActitityId)
                                    .ToList();

            foreach (var activityQueue in activityQueuesGroupedByActivity)
            {
                var currentQueue = activityQueues.Where(x => x.ActitityId == activityQueue.Key);
                var currentQueueCount = currentQueue.Count();

                var queueInfo = GetNewQueueInfo(activityQueue.Key, currentQueueCount, currentQueue);
                queueInfos.Add(queueInfo);
            }

            RecalculateQueueInfos(queueInfos, activityQueues);
            AddQueueInfoForActivitiesWithNoQueue(countryId, companyId, queueInfos, activityQueuesGroupedByActivity);
            RecalculateYourInfo(userId, queueInfos);

            return queueInfos;
        }

        private QueueInfo GetNewQueueInfo(string actitityId, int currentQueueCount, IEnumerable<Queue> currentQueue)
        {
            var queueInfo = new QueueInfo
            {
                ActitityId = actitityId,
                TotalNumbersInQueue = currentQueueCount,
                ActivityQueue = currentQueue
            };
            return queueInfo;
        }

        protected void RecalculateYourInfo(string userId, IEnumerable<QueueInfo> queueInfos)
        {
            foreach (var queueInfo in queueInfos.Where(x => x.TotalNumbersInQueue > 0))
            {
                var queueList = queueInfo.ActivityQueue;
                var yourQueueItem = queueList.FirstOrDefault(x => x.UserId == userId);
                
                if(yourQueueItem != null)
                    queueInfo.YourNumberInQueue = yourQueueItem.NrInQueue;
            }
        }

        protected void RecalculateQueueInfos(IEnumerable<QueueInfo> queueInfos, IEnumerable<Queue> activityQueues)
        {
            var activityWithMaxCountInQueue = GetActivityWithMaxCountInQueue(queueInfos, activityQueues);
            foreach (var queueInfo in queueInfos)
            {
                var progressBarInPercent = GetProgressBarInPercent(queueInfo.TotalWaitTimeInMinutes, activityWithMaxCountInQueue.TotalWaitTimeInMinutes);
                queueInfo.ProgressBarInPercent = progressBarInPercent;
            }
        }

        private void AddQueueInfoForActivitiesWithNoQueue(string countryId, string companyId, List<QueueInfo> queueInfos, List<IGrouping<string, Queue>> activitiesWithQueue)
        {
            var allActivities = _activityData.GetActivities(countryId, companyId);
            var activityWithNoQueue = allActivities.Where(activity => !activitiesWithQueue.Any(queue => queue.Key == activity.Id));

            foreach (var activity in activityWithNoQueue)
            {
                var queueInfo = GetNewQueueInfo(activity.Id, 0, null);
                queueInfos.Add(queueInfo);
            }
        }

        private QueueInfo GetActivityWithMaxCountInQueue(IEnumerable<QueueInfo> queueInfos, IEnumerable<Queue> activityQueues)
        {
            var listWithActivityIdAndMaxCountInQueue = activityQueues
                                    .GroupBy(x => x.ActitityId)
                                    .OrderByDescending(x => x.Count())
                                    .Select(x => new { Id = x.Key, Count = x.Count() })
                                    .ToList();

            var activityIdWithLongestWaitTime = GetActivityIdWithLongestWaitTime(queueInfos);
            var activityIdAndMaxCountInQueue = listWithActivityIdAndMaxCountInQueue.FirstOrDefault();

            var queueInfo = GetNewQueueInfo(activityIdAndMaxCountInQueue.Id, activityIdAndMaxCountInQueue.Count, activityQueues);
            return queueInfo;
        }

        protected string GetActivityIdWithMaxQueueNumbers(IEnumerable<Queue> activityQueues)
        {
            var activityIdWithMaxQueue = activityQueues
                                    .GroupBy(x => x.ActitityId)
                                    .OrderByDescending(x => x.Count())
                                    .First();

            return activityIdWithMaxQueue.Key;
        }

        protected string GetActivityIdWithLongestWaitTime(IEnumerable<QueueInfo> queueInfos)
        {
            var activityIdWithLongestWaitTime = queueInfos
                                                    .OrderByDescending(x => x.TotalWaitTimeInMinutes)
                                                    .FirstOrDefault();
            
            return activityIdWithLongestWaitTime.ActitityId;
        }

        private int GetProgressBarInPercent(int totalWaitTimeInMinutes, int maxWaitTimeInMinutes)
        {
            if (maxWaitTimeInMinutes < 1)
                return 0;

            var percentageStatus = ((double)totalWaitTimeInMinutes / (double)maxWaitTimeInMinutes) * 100;
            return (int)percentageStatus;
        }

        public void RemoveQueueFromQueueInfo(IEnumerable<QueueInfo> queueInfos)
        {
            foreach (var queueInfo in queueInfos)
            {
                queueInfo.ActivityQueue = null;
            }
        }

        public void RemoveQueueFromQueueInfo(List<Activity> activities)
        {
            foreach (var activity in activities)
            {
                if(activity?.QueueInfo?.ActivityQueue != null)
                    activity.QueueInfo.ActivityQueue = null;
            }
        }

        public void AutomaticPopulateQueue()
        {
            if (_queueData.GetActivityQueues().Count == 0)
            {
                QueueAutomaticPopulator.PopulateQueues("1", 10).ToList();
                QueueAutomaticPopulator.PopulateQueues("2", 0).ToList();
                QueueAutomaticPopulator.PopulateQueues("3", 50).ToList();
                QueueAutomaticPopulator.PopulateQueues("4", 99).ToList();
                QueueAutomaticPopulator.PopulateQueues("5", 0).ToList();
                QueueAutomaticPopulator.PopulateQueues("6", 110).ToList();
                QueueAutomaticPopulator.PopulateQueues("7", 15).ToList();
                QueueAutomaticPopulator.PopulateQueues("8", 8).ToList();
                QueueAutomaticPopulator.PopulateQueues("9", 210).ToList();
                QueueAutomaticPopulator.PopulateQueues("10", 10).ToList();
            }
        }
    }

    public static class QueueAutomaticPopulator
    {
        public static IEnumerable<Models.Queue> PopulateQueues(string activityId, int nrOfQueueItems)
        {
            var queueData = new QueueData();

            var models = new List<Models.Queue>();
            for (int i = 1; i < nrOfQueueItems; i++)
            {
                queueData.Add("NOR", "KRSDYREPARK", (1000+i).ToString(), activityId);
            }

            return models;
        }
    }
}

using Bumbleberry.QMeService.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bumbleberry.QMeService.Data
{
    public interface IQueueData
    {
        void Add(string countryId, string companyId, string userId, string activityId);
        List<Models.Queue> GetActivityQueues();
        void Remove(string activityId);
        void RemovePerson(string countryId, string companyId, string activityId, string userId);
        void StoreQueue(List<Models.Queue> activityQueue);
        void Update(string activityId);
    }

    public class QueueData : IQueueData
    {
        public void Add(string countryId, string companyId, string userId, string activityId)
        {
            var activityQueue = CacheHelper.GetActivityQueue().ToList();
            activityQueue.Add(new Models.Queue(countryId, companyId, userId, activityId));
            CacheHelper.SetActivityQueue(activityQueue);
        }

        public void Remove(string activityId)
        {
            Update(activityId);
        }

        //Remove expired persons
        public void Update(string activityId)
        {
            var activityQueue = CacheHelper.GetActivityQueue().ToList();
            var sortedQueue = activityQueue.Where(x => x.ActitityId == activityId).ToList();

            foreach (var person in sortedQueue)
            {
                if (person.TimeAdded < DateTime.Now.AddMinutes(-2))
                {
                    activityQueue.Remove(person);
                    Update(activityId);
                    break;
                }
            }
            StoreQueue(activityQueue);
        }

        public void RemovePerson(string countryId, string companyId, string activityId, string userId)
        {
            var activityQueue = CacheHelper.GetActivityQueue().ToList();
            var personInQueue = activityQueue.FindLast(x => x.CountryId == countryId &&
                                                            x.CompanyId == companyId &&
                                                            x.UserId == userId &&
                                                            x.ActitityId == activityId);
            activityQueue.Remove(personInQueue);
            StoreQueue(activityQueue);
        }

        public List<Models.Queue> GetActivityQueues()
        {
            return CacheHelper.GetActivityQueue().ToList(); ;
        }

        public void StoreQueue(List<Models.Queue> activityQueue)
        {
            CacheHelper.SetActivityQueue(activityQueue);
        }
    }
}

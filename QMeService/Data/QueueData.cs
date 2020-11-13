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
        private static List<Models.Queue> ActivityQueue = new List<Models.Queue>();

        public void Add(string countryId, string companyId, string userId, string activityId)
        {
            ActivityQueue.Add(new Models.Queue(countryId, companyId, userId, activityId));
        }

        public void Remove(string activityId)
        {
            Update(activityId);
        }

        //Remove expired persons
        public void Update(string activityId)
        {
            var sortedQueue = ActivityQueue.Where(x => x.ActitityId == activityId).ToList();

            foreach (var person in sortedQueue)
            {
                if (person.TimeAdded < DateTime.Now.AddMinutes(-2))
                {
                    ActivityQueue.Remove(person);
                    Update(activityId);
                    break;
                }
            }
        }

        public void RemovePerson(string countryId, string companyId, string activityId, string userId)
        {
            var personInQueue = ActivityQueue.FindLast(x => x.CountryId == countryId &&
                                                            x.CompanyId == companyId &&
                                                            x.UserId == userId &&
                                                            x.ActitityId == activityId);
            ActivityQueue.Remove(personInQueue);
        }

        public List<Models.Queue> GetActivityQueues()
        {
            return ActivityQueue;
        }

        public void StoreQueue(List<Models.Queue> activityQueue)
        {
            ActivityQueue = activityQueue;
        }
    }
}

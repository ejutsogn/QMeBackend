using System;
using System.Collections.Generic;
using System.Linq;

namespace Bumbleberry.QMeService.Data
{
    public class QueueData
    {
        private static List<Models.Queue> ActivityQueue = new List<Models.Queue>();
        private static List<Models.Queue> NewActivityQueue = new List<Models.Queue>();

        public void Add(string activityId)
        {
            ActivityQueue.Add(new Models.Queue(activityId));
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

        public void RemovePerson(string activityId)
        {
            var personInQueue = ActivityQueue.FindLast(x => x.ActitityId == activityId);
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

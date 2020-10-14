using System;
using System.Collections.Generic;
using System.Linq;

namespace QService.Data
{
    public class QueueData
    {
        private static List<Model.Queue> ActivityQueue = new List<Model.Queue>();

        public void Add(string activityId)
        {
            ActivityQueue.Add(new Model.Queue(activityId));
        }

        public void Remove(string activityId)
        {
            var sortedQueue = ActivityQueue.Where(x => x.ActitityId == activityId).ToList();

            foreach (var person in sortedQueue)
            {
                if (person.TimeAdded < DateTime.Now.AddMinutes(-2))
                {
                    ActivityQueue.Remove(person);
                    Remove(activityId);
                    break;
                }
            }
        }

        public void RemovePerson(string activityId)
        {
            var personInQueue = ActivityQueue.FindLast(x => x.ActitityId == activityId);
            ActivityQueue.Remove(personInQueue);
        }

        public List<Model.Queue> GetActivityQueues()
        {
            return ActivityQueue;
        }
    }
}

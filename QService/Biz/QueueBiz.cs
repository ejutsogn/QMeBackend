using QService.Data;
using QService.Model;
using System.Collections.Generic;
using System.Linq;

namespace QService.Biz
{
    public class QueueBiz
    {
        QueueData _queueData;

        public QueueBiz()
        {
            _queueData = new QueueData();
        }

        /// <summary>
        /// Person removed from queue after 2 minutes
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public QueueInfo AddInQueue(string activityId)
        {
            _queueData.Remove(activityId);
            _queueData.Add(activityId);
            var queueInfo = GetActivityQueue(activityId);
            
            return queueInfo;
        }

        /// <summary>
        /// Person removed from queue after 2 minutes
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public QueueInfo UpdateQueue(string activityId)
        {
            _queueData.Update(activityId);
            var queueInfo = GetActivityQueue(activityId);

            return queueInfo;
        }

        public QueueInfo RemoveFromQueue(string activityId)
        {
            _queueData.RemovePerson(activityId);
            var queueInfo = GetActivityQueue(activityId);
            return queueInfo;
        }

        public IEnumerable<Model.Queue> GetActivityQueues()
        {
            return _queueData.GetActivityQueues();
        }

        public QueueInfo GetActivityQueue(string activityId)
        {
            var activityQueues = _queueData.GetActivityQueues();
            var sortedQueue = activityQueues.Where(x => x.ActitityId == activityId);

            var queueInfo = new QueueInfo 
            { ActitityId = activityId, NumbersInQueue = sortedQueue.Count(), ActivityQueue = sortedQueue };

            return queueInfo;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Bumbleberry.QMeService.Helper;
using Bumbleberry.QMeService.Models;

namespace Bumbleberry.QMeService.Data
{
    public class QueueData : IQueueData
    {
        public void Add(Queue queue)
        {
            // Add user to db-table QUEUE - For now only add to static object
            var activityQueue = CacheHelper.GetActivityQueue().ToList();
            activityQueue.Add(queue);
            StoreQueue(activityQueue);
        }

        public void RemovePerson(string countryId, string companyGuid, string activityGuid, string userGuid)
        {
            var queue = RemoveFromQueue(countryId, companyGuid, activityGuid, userGuid);
            StoreQueue(queue);
        }

        public void RemoveAllInQueue(string countryId, string companyGuid, string activityGuid)
        {
            var queue = RemoveFromQueue(countryId, companyGuid, activityGuid);
            StoreQueue(queue);
        }

        public void UpdateEstimatedMeetTime(string countryId, string companyGuid, string activityGuid, string userGuid, DateTime estimatedMeetTime)
        {
            var queue = GetActivityQueues(countryId, companyGuid, activityGuid, userGuid).FirstOrDefault();
            queue.EstimatedMeetTime = estimatedMeetTime;
            RemoveFromQueue(countryId, companyGuid, activityGuid, userGuid);
            Add(queue);
        }

        private IEnumerable<Queue> RemoveFromQueue(string countryId, string companyGuid, string activityGuid, string userGuid = "")
        {
            var queue = CacheHelper.GetActivityQueue().ToList();
            if (string.IsNullOrWhiteSpace(userGuid))
            {
                queue.RemoveAll(x => x.CountryId == countryId &&
                            x.CompanyGuid == companyGuid &&
                            x.ActitityGuid == activityGuid);
            }
            else
            {
                queue.RemoveAll(x => x.CountryId == countryId &&
                                x.CompanyGuid == companyGuid &&
                                x.ActitityGuid == activityGuid &&
                                x.UserGuid == userGuid);
            }
            CacheHelper.SetActivityQueue(queue);
            return queue;
        }

        public int GetNumberInQueue(Queue queue)
        {
            if(queue != null)
                return GetNumberInQueue(queue.CountryId, queue.CompanyGuid, queue.ActitityGuid, queue.QueueTime);
            return Constants.DEFAULT_YOUR_NUMBER_IN_QUEUE;
        }

        public int GetNumberInQueue(string countryId, string companyGuid, string activityGuid, DateTime queueTime)
        {
            Validate.ValidateMandatoryFields(countryId, activityGuid);
            Validate.ValidateMandatoryField(activityGuid, "ActivityGuid");
            Validate.ValidateDateTimeHaveValue(queueTime, "QueueTime");

            var queue = CacheHelper.GetActivityQueue().ToList();
            var numberInQueue = queue.Count(x => x.CountryId == countryId &&
                                                    x.CompanyGuid == companyGuid &&
                                                    x.ActitityGuid == activityGuid &&
                                                    x.QueueTime < queueTime);
            return numberInQueue + 1;
        }

        public int GetTotalNumbersInQueue(string countryId, string companyGuid, string activityGuid)
        {
            var queue = CacheHelper.GetActivityQueue().ToList();
            var numbersInQueue = queue.Count(x => x.CountryId == countryId &&
                                                    x.CompanyGuid == companyGuid &&
                                                    x.ActitityGuid == activityGuid);
            return numbersInQueue;
        }

        public IEnumerable<Queue> GetActivityQueues(string countryId, string companyGuid, string activityGuid = "", string userGuid = "")
        {
            var queue = CacheHelper.GetActivityQueue();
            if (string.IsNullOrWhiteSpace(activityGuid) && string.IsNullOrWhiteSpace(userGuid))
            {
                queue = queue.Where(x => x.CountryId == countryId
                                    && x.CompanyGuid == companyGuid);
            }
            else if (!string.IsNullOrWhiteSpace(activityGuid) && string.IsNullOrWhiteSpace(userGuid))
            {
                queue = queue.Where(x => x.CountryId == countryId
                                    && x.CompanyGuid == companyGuid
                                    && x.ActitityGuid == activityGuid);
            }
            else if (string.IsNullOrWhiteSpace(activityGuid) && !string.IsNullOrWhiteSpace(userGuid))
            {
                queue = queue.Where(x => x.CountryId == countryId
                                    && x.CompanyGuid == companyGuid
                                    && x.UserGuid == userGuid);
            }
            else
            {
                queue = queue.Where(x => x.CountryId == countryId
                                    && x.CompanyGuid == companyGuid
                                    && x.ActitityGuid == activityGuid
                                    && x.UserGuid == userGuid);
            }

            return queue;
        }

        public void StoreQueue(IEnumerable<Models.Queue> activityQueue)
        {
            CacheHelper.SetActivityQueue(activityQueue);
        }

        public int GetActivityNumbersPerMinute(string countryId, string companyGuid, string activityGuid)
        {
            // TODO: This must be calculated
            return 6;
        }
    }

    public interface IQueueData
    {
        void Add(Queue queue);
        IEnumerable<Queue> GetActivityQueues(string countryId, string companyGuid, string activityGuid = "", string userGuid = "");
        void RemoveAllInQueue(string countryId, string companyGuid, string activityGuid);
        void RemovePerson(string countryId, string companyGuid, string activityGuid, string userGuid);
        void StoreQueue(IEnumerable<Queue> activityQueue);
        int GetNumberInQueue(string countryId, string companyGuid, string activityGuid, DateTime queueTime);
        int GetNumberInQueue(Queue queue);
        int GetTotalNumbersInQueue(string countryId, string companyGuid, string activityGuid);
        void UpdateEstimatedMeetTime(string countryId, string companyGuid, string activityGuid, string userGuid, DateTime estimatedMeetTime);
    }
}

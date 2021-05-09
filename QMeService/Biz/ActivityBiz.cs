using Bumbleberry.QMeService.Data;
using Bumbleberry.QMeService.Models;
using System.Collections.Generic;
using System.Linq;

namespace Bumbleberry.QMeService.Biz
{
    public class ActivityBiz
    {
        ActivityData _activityData;
        QueueBiz _queueBiz;
        StatusBiz _statusBiz;

        public ActivityBiz()
        {
            _activityData = new ActivityData();
            _queueBiz = new QueueBiz();
            _statusBiz = new StatusBiz();

        }

        public IEnumerable<Activity> GetActivities(string countryId, string companyId, string userId)
        {
            var activities = _activityData.GetActivities(countryId, companyId).ToList();
            UpdateActivityStatusAndQueue(countryId, companyId, userId, activities);
            return activities;
        }

        private void UpdateActivityStatusAndQueue(string countryId, string companyId, string userId, List<Activity> activities)
        {
            foreach (var activity in activities)
            {
                UpdateActivityStatusAndQueue(countryId, companyId, userId, activity);
            }
        }

        private void UpdateActivityStatusAndQueue(string countryGuid, string companyGuid, string userGuid, Activity activity)
        {
            //activity.QueueInfo = _queueBiz.GetActivityQueueInfo(countryGuid, companyGuid, activity.Id, userGuid);
            activity.Status = _statusBiz.GetActivityStatus(activity.Id);
        }

        public Activity GetActivity(string countryId, string companyGuid, string activityGuid, string userGuid)
        {
            var activity = _activityData.GetActivity(countryId, companyGuid, activityGuid);
            UpdateActivityStatusAndQueue(countryId, companyGuid, userGuid, activity);

            return activity;
        }
    }
}

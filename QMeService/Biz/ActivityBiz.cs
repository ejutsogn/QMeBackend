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
            _queueBiz.RemoveQueueFromQueueInfo(activities);
            return activities;
        }

        private void UpdateActivityStatusAndQueue(string countryId, string companyId, string userId, List<Activity> activities)
        {
            foreach (var activity in activities)
            {
                UpdateActivityStatusAndQueue(countryId, companyId, userId, activity);
                _queueBiz.UpdateQueue(countryId, companyId, userId, activity.Id);
            }
        }

        private void UpdateActivityStatusAndQueue(string countryId, string companyId, string userId, Activity activity)
        {
            activity.QueueInfo = _queueBiz.GetActivityQueueInfo(countryId, companyId, userId, activity.Id);
            activity.Status = _statusBiz.GetActivityStatus(activity.Id);
        }

        public Activity GetActivity(string countryId, string companyId, string userId, string activityId)
        {
            var activity = _activityData.GetActivity(activityId);
            UpdateActivityStatusAndQueue(countryId, companyId, userId, activity);

            return activity;
        }
    }
}

using QService.Data;
using QService.Model;
using System.Collections.Generic;
using System.Linq;

namespace QService.Biz
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

        public IEnumerable<Activity> GetActivities(string countryid, string companyid)
        {
            var activities = _activityData.GetActivities(countryid, companyid).ToList();
            UpdateActivityStatusAndQueue(activities);
            return activities;
        }

        private void UpdateActivityStatusAndQueue(List<Activity> activities)
        {
            foreach (var activity in activities)
            {
                UpdateActivityStatusAndQueue(activity);
                _queueBiz.UpdateQueue(activity.Id);
            }
        }

        private void UpdateActivityStatusAndQueue(Activity activity)
        {
            activity.QueueInfo = _queueBiz.GetActivityQueue(activity.Id);
            activity.Status = _statusBiz.GetActivityStatus(activity.Id);
        }

        public Activity GetActivity(string activityId)
        {
            var activity = _activityData.GetActivity(activityId);
            UpdateActivityStatusAndQueue(activity);

            return activity;
        }
    }
}

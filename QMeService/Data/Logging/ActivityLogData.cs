using Bumbleberry.QMeService.Models;
using System;
using System.Collections.Generic;

namespace Bumbleberry.QMeService.Data.Logging
{
    public static class ActivityLogData
    {
        public static void Log(string userId, string method, string description)
        {
            if (StaticDbLog.activityLogs == null)
                StaticDbLog.activityLogs = new List<ActivityLog>();

            var time = DateTime.Now;
            var id = GetNextActivityLogId();
            var activitylogItem = new ActivityLog()
                { Id = id, Method = method, Description = description, CreatedBy = userId, CreatedTime = time };

            StaticDbLog.activityLogs.Add(activitylogItem);
        }

        private static long GetNextActivityLogId()
        {
            long count;
            if (StaticDbLog.activityLogs == null)
                count = 0;
            else
                count = StaticDbLog.activityLogs.Count;
            
            return count + 1;
        }

        public static IEnumerable<ActivityLog> GetLog()
        {
            if (StaticDbLog.activityLogs == null)
                StaticDbLog.activityLogs = new List<ActivityLog>();
            return StaticDbLog.activityLogs;
        }

        public static int GetLogCount()
        {
            if (StaticDbLog.activityLogs == null)
                StaticDbLog.activityLogs = new List<ActivityLog>();
            
            var count = StaticDbLog.activityLogs.Count;
            return count;
        }
    }
}

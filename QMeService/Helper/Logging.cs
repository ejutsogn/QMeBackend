using Bumbleberry.QMeService.Data.Logging;
using System;

namespace Bumbleberry.QMeService.Helper
{
    public class Logging : Attribute
    {
        public Logging(string method, string description = "", string userId = Constants.SYSTEM_USER)
        {
            if(string.IsNullOrWhiteSpace(description))
                description = "Call received";
            ActivityLogData.Log(userId, method, description);
        }
    }
}

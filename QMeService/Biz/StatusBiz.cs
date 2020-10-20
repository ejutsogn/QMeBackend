using Bumbleberry.QMeService.Data;
using Bumbleberry.QMeService.Helper;
using Bumbleberry.QMeService.Models;

namespace Bumbleberry.QMeService.Biz
{
    public class StatusBiz
    {
        public StatusBiz()
        {
            
        }

        public Status GetActivityStatus(string activityId)
        {
            var statusEnum = StatusEnum.Open;
            if(activityId == "3")
                statusEnum = StatusEnum.Closed;
            if (activityId == "7")
                statusEnum = StatusEnum.TechnicalProblem;

            var status = new Status { StatusEnum = statusEnum };
            return status;
        }
    }
}

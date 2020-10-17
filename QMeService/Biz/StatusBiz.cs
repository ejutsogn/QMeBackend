using QMeService.Data;
using QMeService.Helper;
using QMeService.Model;

namespace QMeService.Biz
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

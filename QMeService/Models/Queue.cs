using System;

namespace Bumbleberry.QMeService.Models
{
    public class Queue
    {
        public Queue(string countryId, string companyId, string userId, string actitityId)
        {
            CountryId = countryId;
            CompanyId = companyId;
            UserId = userId;
            ActitityId = actitityId;
            TimeAdded = GetRandomTime();            
        }

        private DateTime GetRandomTime()
        {
            var minExtraSecond = 0;
            var maxExtraSecond = 90;
            var extraSeconds = new Random().Next(minExtraSecond, maxExtraSecond);

            var dateTime = DateTime.Now.AddSeconds(extraSeconds);
            return dateTime;
        }

        public string CountryId { get; set; }
        public string CompanyId { get; set; }
        public string ActitityId { get; set; }
        public string UserId { get; set; }
        public DateTime TimeAdded { get; set; }
        public int NrInQueue { get; set; }
        public int ExtendTimeInMinutes { get; set; }
        public int CountHowManyTimesExtended { get; set; }
    }
}

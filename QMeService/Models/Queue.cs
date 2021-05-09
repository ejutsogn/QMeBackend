using System;

namespace Bumbleberry.QMeService.Models
{
    public class Queue
    {
        private DateTime _queueTime;
        public Queue(string countryId, string companyGuid, string actitityGuid, string userGuid)
        {
            CountryId = countryId;
            CompanyGuid = companyGuid;
            UserGuid = userGuid;
            ActitityGuid = actitityGuid;
            QueueTime = DateTime.Now;//GetRandomTime();
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
        public string CompanyGuid { get; set; }
        public string ActitityGuid { get; set; }
        public string UserGuid { get; set; }
        public int ExtendTimeInMinutes { get; set; }
        public int CountHowManyTimesExtended { get; set; }
        public DateTime QueueTime // Time added in queue
        {
            get
            {
                return _queueTime;
            }
            set
            {
                if (value > _queueTime)
                    _queueTime = value;
            }
        }
        public DateTime EstimatedMeetTime { get; set; }
        public DateTime CheckInTime 
        { get 
            {
                return QueueTime.AddSeconds(120);
            }
        }
        public DateTime BoardingTime
        {
            get
            {
                return QueueTime.AddSeconds(140);
            }
        }
        public bool Deleted { get; set; } = false;
    }
}


using Bumbleberry.QMeService.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bumbleberry.QMeService.Models
{
    public class QueueInfo
    {
        public QueueInfo(string countryId, string companyGuid, string actitityGuid)
        {
            CountryId = countryId;
            CompanyGuid = companyGuid;
            ActitityGuid = actitityGuid;
        }
        public QueueInfo(string countryId, string companyGuid, string actitityGuid, string userGuid)
        {
            CountryId = countryId;
            CompanyGuid = companyGuid;
            ActitityGuid = actitityGuid;
            UserGuid = userGuid;
        }

        public string CountryId { get; }
        public string CompanyGuid { get; }
        public string ActitityGuid { get; }
        public string UserGuid { get; }
        public int TotalNumbersInQueue { get; set; }
        public int NumbersPerMinute { get; set; }
        //public int WaitTimeSecondsPerNumber
        //{
        //    get
        //    {
        //        var waitTime = 60;
        //        if (TotalNumbersInQueue > 200)
        //            waitTime = 45;
        //        else if (TotalNumbersInQueue > 100)
        //            waitTime = 50;
        //        else if (TotalNumbersInQueue > 50)
        //            waitTime = 54;
        //        else if (TotalNumbersInQueue > 20)
        //            waitTime = 56;
        //        else if (TotalNumbersInQueue > 10)
        //            waitTime = 58;
        //        return waitTime;
        //    }
        //    set
        //    {
        //        WaitTimeSecondsPerNumber = value;
        //    }
        //}
        public int ProgressBarInPercent { get; set; }
        public int TotalWaitTimeInMinutes
        {
            get
            {
                var numbersInQueue = TotalNumbersInQueue > 0 ? TotalNumbersInQueue : 1;
                var _numbersPerMinute = NumbersPerMinute > 0 ? NumbersPerMinute : 1;
                double waitTimeInMinutes = numbersInQueue / _numbersPerMinute;
                return (int)Math.Ceiling(waitTimeInMinutes);
            }
        }
        public int YourNumberInQueue { get; set; } = Constants.DEFAULT_YOUR_NUMBER_IN_QUEUE;
        public double YourWaitTimeInMinutes
        {
            get
            {
                var _numbersPerMinute = NumbersPerMinute > 0 ? NumbersPerMinute : 1;
                var waitTime = Math.Ceiling((double)(YourNumberInQueue / _numbersPerMinute));
                if (waitTime < 0)
                    return 0;
                return waitTime;
            }
        }
    }
}

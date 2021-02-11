
using System;
using System.Collections.Generic;

namespace Bumbleberry.QMeService.Models
{
    public class QueueInfo
    {
        public string ActitityId { get; set; }
        public int TotalNumbersInQueue { get; set; }
        public int WaitTimeSecondsPerNumber
        {
            get
            {
                var waitTime = 60;
                if (TotalNumbersInQueue > 200)
                    waitTime = 45;
                else if (TotalNumbersInQueue > 100)
                    waitTime = 50;
                else if (TotalNumbersInQueue > 50)
                    waitTime = 54;
                else if (TotalNumbersInQueue > 20)
                    waitTime = 56;
                else if (TotalNumbersInQueue > 10)
                    waitTime = 58;
                return waitTime;
            }
            set
            {
                WaitTimeSecondsPerNumber = value;
            }
        }
        public int ProgressBarInPercent { get; set; }
        public int TotalWaitTimeInMinutes 
        { 
            get
            {
                var numbersInQueue = TotalNumbersInQueue > 0 ? TotalNumbersInQueue : 1;
                double waitTimeInSeconds = numbersInQueue * WaitTimeSecondsPerNumber;
                var waitTimeInMinutes = Math.Ceiling(waitTimeInSeconds / 60);
                return Convert.ToInt32(waitTimeInMinutes);
            }
        }
        public int YourNumberInQueue { get; set; } = -999;
        public int YourWaitTimeInMinutes 
        {
            get 
            {
                if (YourNumberInQueue <= 5)
                    return 0;

                double waitTimeInSeconds = YourNumberInQueue * WaitTimeSecondsPerNumber;
                var waitTimeInMinutes = Math.Ceiling(waitTimeInSeconds / 60);
                return Convert.ToInt32(waitTimeInMinutes);
            }
        }
        public IEnumerable<Queue> ActivityQueue { get; set; }
    }
}

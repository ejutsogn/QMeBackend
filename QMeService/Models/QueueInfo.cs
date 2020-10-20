
using System;
using System.Collections.Generic;

namespace Bumbleberry.QMeService.Models
{
    public class QueueInfo
    {
        public string ActitityId { get; set; }
        public int TotalNumbersInQueue { get; set; }
        public int YourNumberInQueue { get; set; }
        private int WaitTimeSecondsPerNumber { get; set; } = 60;
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
        public int YourWaitTimeInMinutes { get; set; }
        public IEnumerable<Queue> ActivityQueue { get; set; }
    }
}

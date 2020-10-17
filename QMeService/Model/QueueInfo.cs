
using System;
using System.Collections.Generic;

namespace QMeService.Model
{
    public class QueueInfo
    {
        public string ActitityId { get; set; }
        public int NumbersInQueue { get; set; }
        private int WaitTimeSecondsPerNumber { get; set; } = 60;
        public int TotalWaitTimeInMinutes 
        { 
            get
            {
                var numbersInQueue = NumbersInQueue > 0 ? NumbersInQueue : 1;
                double waitTimeInSeconds = numbersInQueue * WaitTimeSecondsPerNumber;
                var waitTimeInMinutes = Math.Ceiling(waitTimeInSeconds / 60);
                return Convert.ToInt32(waitTimeInMinutes);
            }
        }
        public IEnumerable<Queue> ActivityQueue { get; set; }
    }
}

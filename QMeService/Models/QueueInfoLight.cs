
using System;

namespace Bumbleberry.QMeService.Models
{
    public class QueueInfoLight
    {
        public QueueInfoLight(string actitityId, int totalNumbersInQueue)
        {
            ActitityId = actitityId;
            TotalNumbersInQueue = totalNumbersInQueue;
        }
        public string ActitityId { get; private set; }
        public int TotalNumbersInQueue { get; private set; }
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

        private int WaitTimeSecondsPerNumber
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
    }
}

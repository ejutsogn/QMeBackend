using System;

namespace QMeService.Model
{
    public class Queue
    {
        public Queue(string actitityId)
        {
            ActitityId = actitityId;
            UserId = "";
            TimeAdded = DateTime.Now;
        }
        public string ActitityId { get; set; }
        public string UserId { get; set; }
        public DateTime TimeAdded { get; set; }
        public int NrInQueue { get; set; }
    }
}

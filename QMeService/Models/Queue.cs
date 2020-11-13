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
            TimeAdded = DateTime.Now;
        }
        public string CountryId { get; set; }
        public string CompanyId { get; set; }
        public string ActitityId { get; set; }
        public string UserId { get; set; }
        public DateTime TimeAdded { get; set; }
        public int NrInQueue { get; set; }
    }
}

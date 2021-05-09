using System;

namespace Bumbleberry.QMeService.Models
{

    public class CheckInBoardingTime
    {
        public string CountryId { get; set; }
        public string CompanyGuid { get; set; }
        public string ActitityGuid { get; set; }
        public string UserGuid { get; set; }

        public DateTime CheckInTime
        {
            get; set;                
        }
        public DateTime BoardingTime
        {
            get; set;
        }
    }
}

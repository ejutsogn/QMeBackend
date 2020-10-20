using Bumbleberry.QMeService.Helper;

namespace Bumbleberry.QMeService.Models
{
    public class Status
    {
        public StatusEnum StatusEnum { get; set; }
        public string Description 
        { 
            get 
            {
                return StatusEnum.ToString();
            }
        }
    }
}

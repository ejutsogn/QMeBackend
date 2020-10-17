using QMeService.Helper;

namespace QMeService.Model
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

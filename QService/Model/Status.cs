using QService.Helper;

namespace QService.Model
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

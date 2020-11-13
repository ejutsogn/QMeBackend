using Bumbleberry.QMeService.Helper;

namespace Bumbleberry.QMeService.Models
{
    public class Status
    {
        public StatusEnum StatusEnum { get; set; }
        public string StatusEnumDescription
        { 
            get 
            {
                return StatusEnum.ToString();
            }
        }

        public ItemColors ItemColor 
        {            
            get
            {
                var itemColor = ItemColors.LightGreen;

                switch (StatusEnum)
                {
                    case StatusEnum.Open:
                    case StatusEnum.Opening:
                        itemColor = ItemColors.LightGreen;
                        break;
                    case StatusEnum.Closed:
                    case StatusEnum.OutOfOrder:
                        itemColor = ItemColors.Red;
                        break;
                    case StatusEnum.Closing:
                        itemColor = ItemColors.DarkGreen;
                        break;
                    case StatusEnum.TechnicalProblem:
                        itemColor = ItemColors.Orange;
                        break;
                }

                return itemColor;
            }
        }

        public string ItemColorDescription
        {
            get
            {
                return ItemColor.ToString();
            }
        }
    }
}

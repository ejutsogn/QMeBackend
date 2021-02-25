using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bumbleberry.QMeService.Models
{
    public class ActivityLog
    {
        public long Id { get; set; }
        public string Method { get; set; }
        public string Description { get; set; }
        public DateTime CreatedTime { get; set; }
        public string CreatedBy { get; set; }
    }
}

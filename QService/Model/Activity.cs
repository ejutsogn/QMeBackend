using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QService.Model
{
    public class Activity
    {
        public string Id { get; set; }
        public Company Company { get; set; }
        public string Name { get; set; }
        public string UrlPicture { get; set; }
        public string QrCode { get; set; }
        public string Description { get; set; }
    }
}

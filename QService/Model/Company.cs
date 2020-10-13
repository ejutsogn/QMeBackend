using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QService.Model
{
    public class Company
    {
        public string Id { get; set; }
        public Country Country { get; set; }
        public string Name { get; set; }
    }
}

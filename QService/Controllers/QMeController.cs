using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QService.Biz;
using QService.Model;

namespace QService.Controllers
{
    [ApiController]
    //[Route("[controller]")]
    public class QMeController : ControllerBase
    {
        private readonly ILogger<QMeController> _logger;

        public QMeController(ILogger<QMeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("QMe/GetActivities")]
        public IEnumerable<Activity> GetActivities(string countryid, string companyid)
        {
            var activities = new ActivityBiz().GetActivities(countryid, companyid);
            return activities;
        }

        [HttpGet]
        [Route("QMe/GetCountries")]
        public IEnumerable<Activity> GetCountries()
        {
            var activities = new ActivityBiz().GetActivities(null, null);
            return activities;
        }

        [HttpGet]
        [Route("QMe/AddInQueue")]
        public void AddInQueue()
        {
            var x = "y";
        }

        [HttpGet]
        [Route("QMe/RemoveFromQueue")]
        public void RemoveFromQueue()
        {
            var x = "y";
        }
    }
}

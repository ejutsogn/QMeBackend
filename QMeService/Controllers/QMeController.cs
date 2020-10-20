using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Bumbleberry.QMeService.Biz;
using Bumbleberry.QMeService.Models;

namespace Bumbleberry.QMeService.Controllers
{
    [ApiController]
    //[Route("[controller]")]
    //Attribute routing for REST APIs
    //https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/routing?view=aspnetcore-3.1#ar
    public class QMeController : ControllerBase
    {
        private readonly ILogger<QMeController> _logger;

        public QMeController(ILogger<QMeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("QMe/GetCountries")]
        public IEnumerable<Activity> GetCountries()
        {
            var activities = new ActivityBiz().GetActivities(null, null);
            return activities;
        }

        [HttpGet]
        [Route("QMe/GetCompanies")]
        public IEnumerable<Activity> GetCompanies(string countryId)
        {
            var activities = new ActivityBiz().GetActivities(null, null);
            return activities;
        }

        [HttpGet]
        [Route("QMe/GetActivities")]
        public IEnumerable<Activity> GetActivities(string countryid, string companyid)
        {
            var activities = new ActivityBiz().GetActivities(countryid, companyid);
            return activities;
        }

        [HttpPost]
        [Route("QMe/AddInQueue")]
        public Activity AddInQueue([FromBody] string activityId)
        {
            new QueueBiz().AddInQueue(activityId);
            var activity = new ActivityBiz().GetActivity(activityId);
            return activity;
        }

        [HttpPost]
        [Route("QMe/RemoveFromQueue")]
        public Activity RemoveFromQueue([FromBody] string activityId)
        {
            new QueueBiz().RemoveFromQueue(activityId);
            var activity = new ActivityBiz().GetActivity(activityId);
            return activity;
        }
    }
}

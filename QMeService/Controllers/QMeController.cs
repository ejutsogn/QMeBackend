﻿using System.Collections.Generic;
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
            var activities = new ActivityBiz().GetActivities("","","");
            return activities;
        }

        [HttpGet]
        [Route("QMe/GetCompanies")]
        public IEnumerable<Activity> GetCompanies(string countryId)
        {
            var activities = new ActivityBiz().GetActivities("","","");
            return activities;
        }

        [HttpGet]
        [Route("QMe/GetActivities")]
        public IEnumerable<Activity> GetActivities(string countryId="", string companyId="", string userId="10")
        {
            if (string.IsNullOrEmpty(userId))
                userId = "-999";
            
            var activities = new ActivityBiz().GetActivities(countryId, companyId, userId);
            return activities;
        }

        [HttpPost]
        [Route("QMe/AddInQueue")]
        public Activity AddInQueue([FromBody] string countryId, string companyId, string userId, string activityId)
        {
            if (string.IsNullOrEmpty(userId))
                userId = "-999";
            new QueueBiz().AddInQueue(countryId, companyId, userId, activityId);
            var activity = new ActivityBiz().GetActivity(countryId, companyId, userId, activityId);
            return activity;
        }

        [HttpPost]
        [Route("QMe/RemoveFromQueue")]
        public Activity RemoveFromQueue([FromBody] string countryId, string companyId, string userId, string activityId)
        {
            if (string.IsNullOrEmpty(userId))
                userId = "-999";
            new QueueBiz().RemoveFromQueue(countryId, companyId, userId, activityId);
            var activity = new ActivityBiz().GetActivity(countryId, companyId, userId, activityId);
            return activity;
        }
    }
}
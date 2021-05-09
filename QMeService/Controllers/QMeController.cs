using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Bumbleberry.QMeService.Biz;
using Bumbleberry.QMeService.Models;
using Bumbleberry.QMeService.Data.Logging;
using Bumbleberry.QMeService.Helper;
using System;

//http://www.binaryintellect.net/articles/9db02aa1-c193-421e-94d0-926e440ed297.aspx
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
        public IEnumerable<Activity> GetActivities(string countryId, string companyGuid, string userGuid)
        {
            var activities = new ActivityBiz().GetActivities(countryId, companyGuid, userGuid);
            return activities;
        }

        [HttpPost]
        [Route("QMe/AddInQueue")]
        [Logging("ApiController.AddInQueue()")]
        public QueueInfo AddInQueue([FromBody] string countryId, string companyGuid, string activityGuid, string userGuid)
        {
            var queueInfo = new QueueBiz().AddInQueue(countryId, companyGuid, activityGuid, userGuid);
            return queueInfo;
        }

        [HttpPost]
        [Route("QMe/RemoveFromQueue")]
        [Logging("ApiController.RemoveFromQueue()")]
        public QueueInfo RemoveFromQueue([FromBody] string countryGuid, string companyGuid, string userGuid, string activityGuid)
        {
            var queueInfo = new QueueBiz().RemoveFromQueue(countryGuid, companyGuid, activityGuid, userGuid);
            return queueInfo;
        }

        [HttpGet]
        [Route("QMe/GetQueue")]
        [Logging("ApiController.GetQueue()")]
        public QueueInfo GetQueue(string countryId, string companyGuid, string activityGuid, string userGuid)
        {
            var queueInfo = new QueueBiz().GetActivityQueueInfo(countryId, companyGuid, activityGuid, userGuid);
            return queueInfo;
        }

        [HttpPost]
        [Route("QMe/GetNewUserGuid")]
        [Logging("ApiController.GetNewUserGuid()")]
        public ActionResult<DeviceInfo> GetNewUserGuid([FromBody] DeviceInfo deviceInfo)
        {
            ActivityLogData.Log(Constants.SYSTEM_USER, "ApiController.GetNewUserGuid()", $"Call received");
            var userBiz = new UserBiz();
            var newDeviceInfo = userBiz.GetUserGuid(deviceInfo);
            return Ok(newDeviceInfo);
        }

        [HttpPost]
        [Route("QMe/CreateUserAccount")]
        [Logging("ApiController.CreateUserAccount()")]
        public ActionResult<DeviceInfo> CreateUserAccount([FromBody] DtoCreateUser dtoCreateUser)
        {
            try
            {
                var userBiz = new UserBiz();
                var newDtoCreateUser = userBiz.CreateUserAccount(dtoCreateUser);
                return Ok(newDtoCreateUser);
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }


        // Methods for testing

        [HttpGet]
        [Route("QMe/GetPersons")]
        public IEnumerable<Person> GetPersons()
        {
            return new UserBiz().GetPersons();
        }

        [HttpGet]
        [Route("QMe/GetPersonCount")]
        public int GetPersonCount()
        {
            return new UserBiz().GetPersonCount();
        }

        [HttpGet]
        [Route("QMe/GetDeviceInfos")]
        public IEnumerable<DeviceInfo> GetDeviceInfos()
        {
            return new UserBiz().GetDeviceInfos();
        }

        [HttpGet]
        [Route("QMe/GetDeviceInfoCount")]
        public int GetDeviceInfoCount()
        {
            return new UserBiz().GetDeviceInfoCount();
        }

        [HttpGet]
        [Route("QMe/GetActivityLog")]
        public IEnumerable<ActivityLog> GetActivityLog()
        {
            return ActivityLogData.GetLog();
        }

        [HttpGet]
        [Route("QMe/GetLogCount")]
        public int GetLogCount()
        {
            return ActivityLogData.GetLogCount();
        }
    }
}

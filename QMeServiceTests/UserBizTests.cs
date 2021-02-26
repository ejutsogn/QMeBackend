using Bumbleberry.QMeService.Biz;
using Bumbleberry.QMeService.Data;
using Bumbleberry.QMeService.Data.Log;
using Bumbleberry.QMeService.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Models = Bumbleberry.QMeService.Models;

namespace QMeServiceTests
{
    public class UserBizTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetUserGuid_GuidAlreadyInDeviceInfo()
        {
            var expectedGuid = "1234";

            var deviceInfo = new DeviceInfo() { UserGuid = expectedGuid };

            var newDeviceInfo = new UserBiz().GetUserGuid(deviceInfo);

            Assert.AreEqual(expectedGuid, newDeviceInfo.UserGuid);
        }

        [Test]
        public void GetUserGuid_ReturnNewGuid()
        {
            var deviceInfo = new DeviceInfo() 
                    { UserName = "Per", UserPassword = "Pwd1234", DeviceName = "ThePhone" };

            var actualGuid = new UserBiz().GetUserGuid(deviceInfo);

            Assert.IsTrue(!string.IsNullOrWhiteSpace(actualGuid.UserGuid));
        }

        [Test]
        public void GetUserGuid_Return3NewGuids()
        {
            var deviceInfo1 = new DeviceInfo()
            { UserName = "Per", UserPassword = "Pwd10", DeviceName = "PersMobil" };
            var deviceInfo2 = new DeviceInfo()
            { UserName = "Kari", UserPassword = "Pwd20", DeviceName = "KarisMobil" }; 
            var deviceInfo3 = new DeviceInfo()
            { UserName = "Ola", UserPassword = "Pwd30", DeviceName = "OlasMobil" };

            var actualGuid1 = new UserBiz().GetUserGuid(deviceInfo1);
            var actualGuid2 = new UserBiz().GetUserGuid(deviceInfo2);
            var actualGuid3 = new UserBiz().GetUserGuid(deviceInfo3);

            var log = ActivityLogData.GetLog();

            Assert.IsTrue(!string.IsNullOrWhiteSpace(actualGuid1.UserGuid), "ActualGuid1 is empty");
            Assert.IsTrue(!string.IsNullOrWhiteSpace(actualGuid2.UserGuid), "ActualGuid2 is empty");
            Assert.IsTrue(!string.IsNullOrWhiteSpace(actualGuid3.UserGuid), "ActualGuid3 is empty");

            Assert.IsTrue(actualGuid1 != actualGuid2, "actualGuid1 and actualGuid2 are equal");
            Assert.IsTrue(actualGuid1 != actualGuid3, "actualGuid1 and actualGuid3 are equal");
            Assert.IsTrue(actualGuid2 != actualGuid3, "actualGuid2 and actualGuid3 are equal");

            //Assert.IsTrue(log.Count() == 3, $"Missing records in ActivityLog. Expected 3, found {log.Count()}");
        }
    }
}

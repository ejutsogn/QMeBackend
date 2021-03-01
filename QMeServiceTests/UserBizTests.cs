using Bumbleberry.QMeService.Biz;
using Bumbleberry.QMeService.Data.Logging;
using Bumbleberry.QMeService.Models;
using NUnit.Framework;

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

        [Test]
        public void CreateUserAccount_Return3NewGuids()
        {
            var deviceInfo1 = new DeviceInfo()
            { UserName = "Per", UserPassword = "Pwd10", DeviceName = "PersMobil" };
            var deviceInfo2 = new DeviceInfo()
            { UserName = "Kari", UserPassword = "Pwd20", DeviceName = "KarisMobil" };
            var deviceInfo3 = new DeviceInfo()
            { UserName = "Ola", UserPassword = "Pwd30", DeviceName = "OlasMobil" };
            var person1 = new Person() { FirstName = "Per", LastName = "Larsen", Email = "test1@qme.no" };
            var person2 = new Person() { FirstName = "Kari", LastName = "Olsen", Email = "test1@qme.no" };
            var person3 = new Person() { FirstName = "Ola", LastName = "Normann", Email = "test3@qme.no" };

            var dtoCreateUser1 = new DtoCreateUser() { DeviceInfo = deviceInfo1, Person = person1 };
            var dtoCreateUser2 = new DtoCreateUser() { DeviceInfo = deviceInfo2, Person = person2 };
            var dtoCreateUser3 = new DtoCreateUser() { DeviceInfo = deviceInfo3, Person = person3 };

            var newDtoCreateUser1 = new UserBiz().CreateUserAccount(dtoCreateUser1);
            var newDtoCreateUser2 = new UserBiz().CreateUserAccount(dtoCreateUser2);
            var newDtoCreateUser3 = new UserBiz().CreateUserAccount(dtoCreateUser3);

            var log = ActivityLogData.GetLog();

            Assert.IsTrue(!string.IsNullOrWhiteSpace(newDtoCreateUser1.DeviceInfo.UserGuid), "dtoCreateUser1.DeviceInfo.UserGuid is empty");
            Assert.IsTrue(!string.IsNullOrWhiteSpace(newDtoCreateUser1.Person.UserGuid), "dtoCreateUser1.Person.UserGuid is empty");
            Assert.IsTrue(!string.IsNullOrWhiteSpace(newDtoCreateUser2.DeviceInfo.UserGuid), "dtoCreateUser2.DeviceInfo.UserGuid is empty");
            Assert.IsTrue(!string.IsNullOrWhiteSpace(newDtoCreateUser2.Person.UserGuid), "dtoCreateUser2.Person.UserGuid is empty");
            Assert.IsTrue(!string.IsNullOrWhiteSpace(newDtoCreateUser3.DeviceInfo.UserGuid), "dtoCreateUser3.DeviceInfo.UserGuid is empty");
            Assert.IsTrue(!string.IsNullOrWhiteSpace(newDtoCreateUser3.Person.UserGuid), "dtoCreateUser3.Person.UserGuid is empty");

            Assert.IsTrue(newDtoCreateUser1.DeviceInfo.UserGuid == newDtoCreateUser1.Person.UserGuid, "Error checking guid in DtoCreateUser1");
            Assert.IsTrue(newDtoCreateUser2.DeviceInfo.UserGuid == newDtoCreateUser2.Person.UserGuid, "Error checking guid in DtoCreateUser2");
            Assert.IsTrue(newDtoCreateUser3.DeviceInfo.UserGuid == newDtoCreateUser3.Person.UserGuid, "Error checking guid in DtoCreateUser3");
            Assert.IsTrue(newDtoCreateUser1.DeviceInfo.UserGuid != newDtoCreateUser2.DeviceInfo.UserGuid, "Error checking guid for 1 & 2");
            Assert.IsTrue(newDtoCreateUser1.DeviceInfo.UserGuid != newDtoCreateUser3.DeviceInfo.UserGuid, "Error checking guid for 1 & 3");
            Assert.IsTrue(newDtoCreateUser2.DeviceInfo.UserGuid != newDtoCreateUser3.DeviceInfo.UserGuid, "Error checking guid for 2 & 3");
        }
    }
}

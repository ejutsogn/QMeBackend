using Bumbleberry.QMeService.Data.Logging;
using Bumbleberry.QMeService.Models;
using System.Collections.Generic;
using System.Linq;

namespace Bumbleberry.QMeService.Data
{
    public class UserData
    {

        public void StoreDeviceInfo(DeviceInfo deviceInfo)
        {
            ActivityLogData.Log(deviceInfo.UserGuid, "UserData.StoreDeviceInfo()", $"Store DeviceInfo");

            if (StaticDb.DeviceInfos == null)
                StaticDb.DeviceInfos = new List<DeviceInfo>();

            StaticDb.DeviceInfos.Add(deviceInfo);
        }

        public void StorePerson(Person person)
        {
            ActivityLogData.Log(person.UserGuid, "UserData.StorePerson()", $"Store Person");

            if (StaticDb.Persons == null)
                StaticDb.Persons = new List<Person>();

            StaticDb.Persons.Add(person);
        }

        public DeviceInfo GetDeviceInfo(string guid)
        {
            if (StaticDb.DeviceInfos == null)
                return null;
            var deviceInfo = StaticDb.DeviceInfos.FirstOrDefault(x => x.UserGuid == guid);

            return deviceInfo;
        }

        public Person GetPerson(string guid)
        {
            if (StaticDb.Persons == null)
                return null;
            var person = StaticDb.Persons.FirstOrDefault(x => x.UserGuid == guid);

            return person;
        }

        public IEnumerable<DtoCreateUser> GetPersonDeviceInfos()
        {
            return null;
        }

        public IEnumerable<DeviceInfo> GetDeviceInfos()
        {
            return StaticDb.DeviceInfos;
        }

        public IEnumerable<Person> GetPersons()
        {
            return StaticDb.Persons;
        }
    }
}

using Bumbleberry.QMeService.Data.Log;
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

            if (StaticDb.deviceInfos == null)
                StaticDb.deviceInfos = new List<DeviceInfo>();

            StaticDb.deviceInfos.Add(deviceInfo);
        }

        public DeviceInfo GetDeviceInfo(string guid)
        {
            if (StaticDb.deviceInfos == null)
                return null;
            var deviceInfo = StaticDb.deviceInfos.FirstOrDefault(x => x.UserGuid == guid);

            return deviceInfo;
        }

        public IEnumerable<DeviceInfo> GetDeviceInfos()
        {
            return StaticDb.deviceInfos;
        }
    }
}

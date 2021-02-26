using Bumbleberry.QMeService.Data;
using Bumbleberry.QMeService.Data.Logging;
using Bumbleberry.QMeService.Models;
using System;
using System.Collections.Generic;

namespace Bumbleberry.QMeService.Biz
{
    public class UserBiz
    {
        public DeviceInfo GetUserGuid(DeviceInfo deviceInfo)
        {
            var newDeviceInfo = deviceInfo;
            if (!string.IsNullOrWhiteSpace(newDeviceInfo.UserGuid))
            {
                ActivityLogData.Log(newDeviceInfo.UserGuid, "UserBiz.GetUserGuid()", $"UserGuid already exists in received DeviceInfo");
                return newDeviceInfo;
            }

            var guid = GenerateGuid();

            while (!IsUniqueUserGuid(guid))
            {
                guid = GenerateGuid();
            }

            newDeviceInfo.UserGuid = guid;
            new UserData().StoreDeviceInfo(newDeviceInfo);

            return newDeviceInfo;
        }

        private bool IsUniqueUserGuid(string guid)
        {
            var deviceInfo = new UserData().GetDeviceInfo(guid);
            if (deviceInfo != null && deviceInfo.UserGuid == guid)
                return false;
            
            return true;
        }

        private string GenerateGuid()
        {
            //var guid = Guid.NewGuid().ToString();

            var random = new Random();
            var guid = random.Next(1, 9).ToString() + random.Next(1, 9).ToString();

            return guid;
        }

        public IEnumerable<DeviceInfo> GetDeviceInfos()
        {
            return new UserData().GetDeviceInfos();
        }
    }
}

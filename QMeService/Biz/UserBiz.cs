using Bumbleberry.QMeService.Data;
using Bumbleberry.QMeService.Data.Logging;
using Bumbleberry.QMeService.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bumbleberry.QMeService.Biz
{
    public class UserBiz
    {
        public DeviceInfo GetUserGuid(DeviceInfo deviceInfo)
        {
            var newDeviceInfo = deviceInfo;
            if (!string.IsNullOrWhiteSpace(newDeviceInfo.UserGuid))
            {
                ActivityLogData.Log(newDeviceInfo.UserGuid, "UserBiz.GetUserGuid()", $"UserGuid({newDeviceInfo.UserGuid}) found in received DeviceInfo");
                if(IsUniqueUserGuid(newDeviceInfo.UserGuid))
                    return newDeviceInfo;
                else
                    LogActivityCreateGuid(newDeviceInfo, $"UserGuid({newDeviceInfo.UserGuid}) not valid - Generating new");
            }

            var guid = GenerateGuid();
            newDeviceInfo.CreatedTime = DateTime.Now;

            while (!IsUniqueUserGuid(guid))
            {
                guid = GenerateGuid();
            }

            newDeviceInfo.UserGuid = guid;
            new UserData().StoreDeviceInfo(newDeviceInfo);
            LogActivityCreateGuid(newDeviceInfo, $"Generated GUID: {newDeviceInfo.UserGuid}");

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

        public int GetDeviceInfoCount()
        {
            return new UserData().GetDeviceInfos().Count();
        }

        private void LogActivityCreateGuid(DeviceInfo newDeviceInfo, string appendMsg)
        {
            string msg = "";
            if (!string.IsNullOrWhiteSpace(newDeviceInfo.DeviceName))
                msg = $"{msg} - DeviceName: {newDeviceInfo.DeviceName}";
            if (!string.IsNullOrWhiteSpace(newDeviceInfo.Model))
                msg = $"{msg} - Model: {newDeviceInfo.Model}";
            if (!string.IsNullOrWhiteSpace(newDeviceInfo.UserName))
                msg = $"{msg} - UserName: {newDeviceInfo.UserName}";

            msg = $"{msg} - {appendMsg}";

            ActivityLogData.Log(newDeviceInfo.UserGuid, "UserBiz.LogActivityCreateGuid()", msg);
        }
    }
}

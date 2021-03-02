using Bumbleberry.QMeService.Data;
using Bumbleberry.QMeService.Data.Logging;
using Bumbleberry.QMeService.Helper;
using Bumbleberry.QMeService.Models;
using QMeFrontend.Helper;
using QMeFrontend.Models;
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
                ActivityLogData.Log(newDeviceInfo.UserGuid, "UserBiz.GetUserGuid()", $"UserGuid({newDeviceInfo.UserGuid}) found in received DeviceInfo - Returning if its unique");
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

        public DtoCreateUser CreateUserAccount(DtoCreateUser dtoCreateUser)
        {
            var validate = ValidateCreateUserAccount(dtoCreateUser);
            if(!validate.Status)
            {
                ActivityLogData.Log(Constants.SYSTEM_USER, "UserBiz.CreateUserAccount()", $"Validation failed - {validate.Message}");
                throw new Exception(validate.Message);
            }

            var newDtoCreateUser = dtoCreateUser;
            var newDeviceInfo = GetUserGuid(newDtoCreateUser.DeviceInfo);
            newDtoCreateUser.DeviceInfo = newDeviceInfo;

            var person = newDtoCreateUser.Person;
            person.UserGuid = newDeviceInfo.UserGuid;
            CreatePerson(person);

            ActivityLogData.Log(newDtoCreateUser.DeviceInfo.UserGuid, "UserBiz.CreateUserAccount()", $"UserGuid({newDtoCreateUser.DeviceInfo.UserGuid}) Created");

            return newDtoCreateUser;
        }

        private ReturnStatus ValidateCreateUserAccount(DtoCreateUser dtoCreateUser)
        {
            var returnStatus = new ReturnStatus();
            if(dtoCreateUser.DeviceInfo == null || dtoCreateUser.Person == null)
            {
                returnStatus.Status = false;
                returnStatus.Message = ErrorHelper.SetSystemErrorMessage("Error calling CreateUserAccount(). DeviceInfo/Person is null") + ErrorHelper.SetUserFriendlyErrorMessage("Error creating user");
            }

            if (string.IsNullOrWhiteSpace(dtoCreateUser.Person.FirstName))
            {
                returnStatus.Status = false;
                returnStatus.Message = ErrorHelper.SetUserFriendlyErrorMessage("First name is mandatory");
            }

            if (string.IsNullOrWhiteSpace(dtoCreateUser.Person.LastName))
            {
                returnStatus.Status = false;
                returnStatus.Message = ErrorHelper.SetUserFriendlyErrorMessage("Last name is mandatory");
            }

            if (string.IsNullOrWhiteSpace(dtoCreateUser.Person.Email))
            {
                returnStatus.Status = false;
                returnStatus.Message = ErrorHelper.SetUserFriendlyErrorMessage("Email is mandatory");
            }

            if (string.IsNullOrWhiteSpace(dtoCreateUser.DeviceInfo.UserPassword))
            {
                returnStatus.Status = false;
                returnStatus.Message = ErrorHelper.SetUserFriendlyErrorMessage("Password is mandatory");
            }
            return returnStatus;
        }

        private void CreatePerson(Person person)
        {
            new UserData().StorePerson(person);
        }

        private bool IsUniqueUserGuid(string guid)
        {
            var deviceInfo = new UserData().GetDeviceInfo(guid);
            if (deviceInfo != null && deviceInfo.UserGuid == guid)
                return false;

            var person = new UserData().GetPerson(guid);
            if (person != null && person.UserGuid == guid)
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

        public IEnumerable<Person> GetPersons()
        {
            return new UserData().GetPersons();
        }

        public int GetPersonCount()
        {
            return new UserData().GetPersons().Count();
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

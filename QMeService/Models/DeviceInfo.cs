
using System;

namespace Bumbleberry.QMeService.Models
{
    public class DeviceInfo
    {
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public string DeviceName { get; set; }
        public string OperationSystemVersion { get; set; }
        public string DevicePlatform { get; set; }
        public string TypeOfDevice { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string UserGuid { get; set; }
        public DateTime CreatedTime { get; set; }
        //public Person Person { get; set; }
    }
}

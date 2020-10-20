using Bumbleberry.QMeService.Helper;

namespace Bumbleberry.QMeService.Models
{
    public class Address
    {
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public Country Country { get; set; }
        public AddressType AddressType { get; set; }
    }
}

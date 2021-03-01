using Bumbleberry.QMeService.Helper;

namespace Bumbleberry.QMeService.Models
{
    public class Person
    {
        public string UserGuid { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Sex Sex { get; set; }
        public int Age { get; set; }
        public string Telephone { get; set; }
        public string CountryCode { get; set; }
        public Address Address { get; set; }
    }
}

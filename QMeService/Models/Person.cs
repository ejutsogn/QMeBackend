using Bumbleberry.QMeService.Helper;

namespace Bumbleberry.QMeService.Models
{
    public class Person
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public Sex Sex { get; set; }
        public int Age { get; set; }
        public string Telephone { get; set; }
        public Address Address { get; set; }
    }
}


namespace Bumbleberry.QMeService.Models
{
    public class Company
    {
        public string Id { get; set; }
        public Country Country { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Telephone { get; set; }
        public Person ContactPerson { get; set; }
    }
}

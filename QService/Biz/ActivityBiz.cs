using QService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QService.Biz
{
    public class ActivityBiz
    {
        public IEnumerable<Activity> GetActivities(string countryid, string companyid)
        {
            return PopulateActivities(countryid, companyid);
        }

        private IEnumerable<Activity> PopulateActivities(string countryid, string companyid)
        {
            var country = new Country { Id = "1", Name = "Norge" };
            var company = new Company { Id = "1", Name = "Kristiansand Dyrepark", Country = country };

            var activities = new List<Activity>();
            var activity = GetActivity("1", "Nilen", "Tømmerrenne der du seiler rundt...", company);
            activities.Add(activity);
            activity = GetActivity("2", "Jungelbob", "Bobbane for den tøffe...", company);
            activities.Add(activity);
            activity = GetActivity("3", "Kaptein Sabeltanns skute", "Opplev hvordan det er å være sjørøver...", company);
            activities.Add(activity);

            return activities;
        }

        private Activity GetActivity(string id, string name, string description, Company company)
        {
            return new Activity { Id = id, Name = name, Description = description, Company = company };
        }

        private Company GetCompany(string id, string name, Country country)
        {
            return new Company { Id = id, Name = name, Country = country };
        }

        private Country GetCountry(string id, string name)
        {
            return new Country { Id = id, Name = name };
        }
    }
}

using Bumbleberry.QMeService.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bumbleberry.QMeService.Data
{
    public class ActivityData
    {

        public IEnumerable<Activity> GetActivities(string countryid, string companyGuid)
        {
            var activities = PopulateActivities();
            var filteredActivities = activities.Where(x => x.CountryId == countryid && x.CompanyGuid == companyGuid);

            return filteredActivities;
        }

        public Activity GetActivity(string countryid, string companyGuid, string activityGuid)
        {
            var activities = PopulateActivities();
            var activity = activities.FirstOrDefault(x => x.CountryId == countryid 
                                                        && x.CompanyGuid == companyGuid
                                                        && x.Id == activityGuid);
            return activity;
        }

        private IEnumerable<Activity> PopulateActivities()
        {
            var activities = PopulateActivitiesForKrsDyrepark().ToList();
            activities.AddRange(PopulateActivitiesForKongeparken());
            return activities;
        }

        private IEnumerable<Activity> PopulateActivitiesForKongeparken()
        {
            var country = GetNewCountry("1", "Norge");
            var company = GetNewCompany("2", "Kongeparken", country);

            var activities = new List<Activity>();
            var activity = GetNewActivity("act1", "Humla", "Surr rundt med Humla.", company, "humla.png");
            activities.Add(activity);
            activity = GetNewActivity("act2", "Ormen Lange", "Ormen Lange er Norges lengste bob-bane. Her sitter dere to og to på kjelker som renner i stor fart ned en bob-bane. Du har full kontroll på fart og brems, men det kiler godt i magen når du slipper opp i de bratteste svingene.", company, "ornemLange.png");
            activities.Add(activity);
            activity = GetNewActivity("act3", "Turbo knallerten", "Kjør i vei både høyt og lavt", company, "turboknallerten.png");
            activities.Add(activity);

            return activities;
        }

        private IEnumerable<Activity> PopulateActivitiesForKrsDyrepark()
        {
            var country = GetNewCountry("1", "Norge");
            var company = GetNewCompany("1", "Kristiansand Dyrepark", country);

            var activities = new List<Activity>();
            var activity = GetNewActivity("act1", "Nilen", "Unn deg en forfriskende tur langs Nilen.", company, "nilen.png");
            activities.Add(activity);
            activity = GetNewActivity("act2", "Jungelbob", "JungelBob er en kjempekul bob-bane. Her sitter dere to og to på kjelker som renner i stor fart ned en bob-bane. Du har full kontroll på fart og brems, men det kiler godt i magen når du slipper opp i de bratteste svingene.", company, "jungelbob.png");
            activities.Add(activity);
            activity = GetNewActivity("act3", "Kaptein Sabeltanns skute", "Opplev hvordan det er å være sjørøver...", company, "kapteinsabeltannskute.png");
            activities.Add(activity);
            activity = GetNewActivity("act4", "Spøkelseshuset", "I Kaptein Sabeltanns mørkeste mareritt sender Spøkelseshusets portvokter deg inn i drømmenes og marerittenes verden. Selv Kongen på Havet er redd noe, og alt det han frykter er samlet innenfor dette husets vegger.", company, "spokelseshuset.png");
            activities.Add(activity);
            activity = GetNewActivity("act5", "Reli Safari", "Reli Safari er et fint lite lokomotiv som kjører i rute rundt Manyatta – Barnas Afrikanske Landsby.", company, "relisafari.png");
            activities.Add(activity);
            activity = GetNewActivity("act6", "Karusellen", "Nostalgisk gammeldags karusell. Dette er en kjempefin aktivitet for de minste.", company, "karusell.png");
            activities.Add(activity);
            activity = GetNewActivity("act7", "Mosks traktorbane", "Tenk å få lov til å kjøre traktor helt på egen hånd! På KuToppen i Dyreparken kan alle uansett alder få kjøre en tur i Mosks Traktorbane.", company, "moskstraktor.png");
            activities.Add(activity);
            activity = GetNewActivity("act8", "Kaffekoppen", "Karusell med snurrende vogner som ser ut som kaffekopper. Her får du utfordre svimmelheten din, men dette går ikke veldig fort, og passer veldig fint for små barn som synes det er moro å snurre rundt.", company, "kaffekoppen.png");
            activities.Add(activity);
            activity = GetNewActivity("act9", "Ubåtene", "Bli med på en liten flytur i en ubåt. Dette er en morsom liten karusell for små barn.", company, "ubaatene.png");
            activities.Add(activity);
            activity = GetNewActivity("act10", "Bilbanen", "Elektriske minibiler for barn over 4 år.", company, "bilbanen.png");
            activities.Add(activity);

            return activities;
        }

        private Activity GetNewActivity(string id, string name, string description, Company company, string imageName = "")
        {
            var random3_8 = new Random().Next(3, 8);
            var imageUrl = $"/Assets/Images/Norway/KRSDyrepark/{imageName}";
            return new Activity { Id = id, Name = name, Description = description, CountryId = company.Country.Id, CompanyGuid = company.Id, UrlPicture = imageUrl, NumbersPerMinute = random3_8 };
        }

        private Company GetNewCompany(string id, string name, Country country)
        {
            return new Company { Id = id, Name = name, Country = country };
        }

        private Country GetNewCountry(string id, string name)
        {
            return new Country { Id = id, Name = name };
        }
    }
}

using Bumbleberry.QMeService.Data;
using Bumbleberry.QMeService.Helper;
using Bumbleberry.QMeService.Models;
using System;
using System.Collections.Generic;
using System.Linq;

/*
 * QueueTime => Brukes alltid for å finne kundens plass i køen
 * -Kunden kan flytte seg bakover i køen(Ikke i versjon 1)
 * CountHowManyTimesExtended => Teller hvor mange ganger kunden har utsatt plassen(Ikke i versjon 1)
 * Deleted => True: Kunden er helt ute av køen(slettet seg selv eller tid utgått)
 * #Biz-rules: (ikke versjon 1)
 * -Kunden må ha min 10 min igjen av ventetiden for å kunne utsette tiden
 * -Kunden må utsette tiden med minimum 20 min og maks 60 min
 * -Kunden kan utsette tiden 3 ganger
 * -QueueTime angir tidspunkt kunden ble lagt i køen -> Ved utsettelse så flyttes denne tiden og kunden legges mellom andre kunder
 * -EstimatedMeetTime er estimert tid for oppmøte ved attraksjon -> Justeres jevling, usikker på intervall
 * 
#KLASSEN: QUEUE 
- Inneholder informasjon om 1 kunde
- Liste med queue er køen for 1 aktivitet
Id, CountryId, CompanyGuid, ActivityGuid, UserGuid, QueueTime, CountHowManyTimesExtended, Deleted, CreatedTime, LastUpdatedTime, CreatedBy, LastUpdatedBy

10, 47, 1, 10, 77, '07.03.2021 - 10:01:59'
20, 47, 1, 10, 88, '07.03.2021 - 10:02:11'
14, 47, 1, 10, 55, '07.03.2021 - 10:02:15'
71, 47, 1, 10, 99, '07.03.2021 - 10:04:08'
72, 47, 1, 10, 88, '07.03.2021 - 10:04:09'

120 personer i timen -> 2 personer i minuttet
Legg i kø:
- Sjekke antall som er i kø
- QueueTime.QueueTime = DateTime.Now
- QueueTime.EstimatedMeetTime = (antall i kø * antall i minuttet) + QueueTime

Justere tid: Tiden per kunde øker/færre personer per minutt
- Finne antall foran deg i køen: Antall = count QueueTime < UserGuid.QueueTime
- Sjekker QueueInfo.NumbersPerMinute - Hvis ny NumbersPerMinute så må kø-tiden justeres
- Queue.EstimatedMeetTime = Queue.QueueTime + (Count numbers before you in quene / QueueInfo.NumbersPerMinute) 
E.G.: 
QueueTime: 12:00
numbers before you in quene: 10
QueueInfo.NumbersPerMinute: 2
Queue.EstimatedMeetTime = 12:05 -> 12:00 + ( 10 /2 )

Justere tid: Person går ut av køen -> Justeres jevnlig, ca hvert 5 min..?
- Person går bak i køen -> kun justere de som er foran
- Finne alle som skal kø som skal justeres
- Trekke fra kø-tid til 1 person: 120 personer i timen -> 2 personer i minuttet -> trekke fra 30 sekunder

Justere tid: Teknisk feil
- Personer sjekker inn men validerer ikke at de går ombord på aktivitet
- Utsette tiden med X minutter for alle sammen

#KLASSEN: QUEUEINFO
- Inneholder overordnet informasjon for kø til en aktivitet
- Angir total ventetid i min, ventetid per bruker i min, totalt antall i kø, progressbar i %
- Angir ditt nummer i køen
- Liste med QueueInfo brukes for å vise ventetid i aktivitetslisten i appen
Id, CountryId, CompanyGuid, ActivityGuid, NumbersPerMinute

*/

namespace Bumbleberry.QMeService.Biz
{
    public class QueueBiz
    {
        IQueueData _queueData;
        ActivityData _activityData;

        public QueueBiz()
        {
            _queueData = new QueueData();
            _activityData = new ActivityData();
            AutomaticPopulateQueue();
        }

        public QueueBiz(IQueueData queueData)
        {
            _queueData = queueData;
            _activityData = new ActivityData();
        }

        /// <summary>
        /// Add person
        /// * Person removed from queue 5 minutes after EstimatedMeetTime
        /// </summary>
        /// <param name="activityGuid"></param>
        /// <returns></returns>
        public QueueInfo AddInQueue(string countryId, string companyGuid, string activityGuid, string userGuid)
        {
            RemoveExpiredPersons(countryId, companyGuid, activityGuid);
            var queue = GetNewQueue(countryId, companyGuid, activityGuid, userGuid);            
            _queueData.Add(queue);
            var queueInfo = GetNewQueueInfo(queue);
            CalculateQueueEstimatedMeetTime(queueInfo, queue);

            return queueInfo;
        }

        private void CalculateQueueEstimatedMeetTime(QueueInfo queueInfo, Queue queue)
        {
            double estimatedAddTime = queueInfo.YourNumberInQueue / queueInfo.NumbersPerMinute;
            var estimatedMeetTime = queue.QueueTime.AddMinutes((int)Math.Ceiling(estimatedAddTime));
            _queueData.UpdateEstimatedMeetTime(queue.CountryId, queue.CompanyGuid, queue.ActitityGuid, queue.UserGuid, estimatedMeetTime);
        }

        private Queue GetNewQueue(string countryId, string companyGuid, string activityGuid, string userGuid)
        {
            var queue = new Queue(countryId, companyGuid, activityGuid, userGuid);
            return queue;
        }

        private QueueInfo GetNewQueueInfo(Queue queue)
        {
            var queueInfo = new QueueInfo(queue.CountryId, queue.CompanyGuid, queue.ActitityGuid, queue.UserGuid);
            queueInfo.YourNumberInQueue = _queueData.GetNumberInQueue(queue);
            queueInfo.TotalNumbersInQueue = _queueData.GetTotalNumbersInQueue(queue.CountryId, queue.CompanyGuid, queue.ActitityGuid);
            var activity = _activityData.GetActivity(queue.CountryId, queue.CompanyGuid, queue.ActitityGuid);
            queueInfo.NumbersPerMinute = activity.NumbersPerMinute;
            var queueInfos = PopulateGenericQueueInfoList(queue.CountryId, queue.CompanyGuid);
            queueInfo.ProgressBarInPercent = queueInfos.FirstOrDefault(x => x.ActitityGuid == queueInfo.ActitityGuid).ProgressBarInPercent;
            return queueInfo;
        }

        // Remove where EstimatedMeetTime is expired with X minutes
        // E.G.:
        // EstimatedMeetTime = 12:05
        // DateTime.Now = 12:11
        // MINUTES_EXPIRED_BEFORE_REMOVED_FROM_QUEUE = 5
        // Henter liste som skal fjernes: 12:05 < (12:11 - 5)
        // TODO: Compare only hour-minute, not seconds
        private void RemoveExpiredPersons(string countryGuid, string companyGuid, string activityGuid)
        {
            lock (this)
            {
                var queue = _queueData.GetActivityQueues(countryGuid, companyGuid, activityGuid);
                var expiredPersons = queue.Where(x => x.EstimatedMeetTime < DateTime.Now.AddMinutes(-1 * Constants.MINUTES_EXPIRED_BEFORE_REMOVED_FROM_QUEUE));

                var updatedList = queue.Except(expiredPersons);
                _queueData.StoreQueue(updatedList);
            }            
        }

        public QueueInfo RemoveFromQueue(string countryGuid, string companyGuid, string activityGuid, string userGuid)
        {
            _queueData.RemovePerson(countryGuid, companyGuid, activityGuid, userGuid);
            var queueInfo = GetActivityQueueInfo(countryGuid, companyGuid, activityGuid, userGuid);
            return queueInfo;
        }

        public QueueInfo GetActivityQueueInfo(string countryId, string companyGuid, string activityGuid, string userGuid)
        {
            return GetQueueInfos(countryId, companyGuid, userGuid).FirstOrDefault();
        }

        protected IEnumerable<QueueInfo> GetQueueInfos(string countryId, string companyGuid, string userGuid)
        {
            Validate.ValidateMandatoryFields(countryId, companyGuid);
            Validate.ValidateMandatoryField(userGuid, "UserGuid");
            var genericQueueInfos = PopulateGenericQueueInfoList(countryId, companyGuid);

            var yourQueueInfo = UpdateGenericQueueInfoListWithUserSpecificData(countryId, companyGuid, userGuid, genericQueueInfos);

            return yourQueueInfo;
        }

        private IEnumerable<QueueInfo> UpdateGenericQueueInfoListWithUserSpecificData(string countryId, string companyGuid, string userGuid, IEnumerable<QueueInfo> genericQueueInfos)
        {
            var yourActivityQueues = _queueData.GetActivityQueues(countryId, companyGuid, "", userGuid);
            var yourQueueInfos = genericQueueInfos.Where(qi => yourActivityQueues.Any(aq => qi.ActitityGuid == aq.ActitityGuid));

            foreach (var yourQueueInfo in yourQueueInfos)
            {
                var yourActivityQueue = yourActivityQueues.FirstOrDefault(x => x.ActitityGuid == yourQueueInfo.ActitityGuid);
                yourQueueInfo.YourNumberInQueue = _queueData.GetNumberInQueue(yourActivityQueue);
            }
            return yourQueueInfos;
        }

        private IEnumerable<QueueInfo> PopulateGenericQueueInfoList(string countryId, string companyGuid)
        {
            var queueInfos = new List<QueueInfo>();
            var actitivites = _activityData.GetActivities(countryId, companyGuid);
            var queues = _queueData.GetActivityQueues(countryId, companyGuid);
            var queuesGroupedByActivities = queues.GroupBy(x => x.ActitityGuid);

            foreach (var activityQueue in queuesGroupedByActivities)
            {
                var queueInfo = new QueueInfo(countryId, companyGuid, activityQueue.Key);
                var activity = actitivites.FirstOrDefault(x => x.Id == activityQueue.Key);
                queueInfo.TotalNumbersInQueue = activityQueue.Count();
                queueInfo.NumbersPerMinute = activity == null ? 0 : activity.NumbersPerMinute;

                queueInfos.Add(queueInfo);
            }
            queueInfos = CalculateProgressBarInPercent(queueInfos).ToList();

            return queueInfos;
        }

        private IEnumerable<QueueInfo> CalculateProgressBarInPercent(List<QueueInfo> queueInfos)
        {
            var maxInQueue = queueInfos.Max(x => x.TotalNumbersInQueue);
            foreach (var queueInfo in queueInfos)
            {
                var totalInQueue = queueInfo.TotalNumbersInQueue;
                double progressbar = ((double)totalInQueue / (double)maxInQueue) * 100;
                queueInfo.ProgressBarInPercent = (int)progressbar;
            }
            return queueInfos;
        }


        protected string GetActivityIdWithMaxQueueNumbers(IEnumerable<Queue> activityQueues)
        {
            var activityIdWithMaxQueue = activityQueues
                                    .GroupBy(x => x.ActitityGuid)
                                    .OrderByDescending(x => x.Count())
                                    .First();

            return activityIdWithMaxQueue.Key;
        }

        public void RemoveQueueFromQueueInfo(List<Activity> activities)
        {
            foreach (var activity in activities)
            {
                //if(activity?.QueueInfo?.ActivityQueue != null)
                //    activity.QueueInfo.ActivityQueue = null;
            }
        }

        public void AutomaticPopulateQueue()
        {
            if (_queueData.GetActivityQueues("1", "1").Count() == 0)
            {
                QueueAutomaticPopulator.PopulateQueues("1", 10).ToList();   // ActivityId, NumberOfPersonsInQueue
                QueueAutomaticPopulator.PopulateQueues("2", 0).ToList();
                QueueAutomaticPopulator.PopulateQueues("3", 50).ToList();
                QueueAutomaticPopulator.PopulateQueues("4", 99).ToList();
                QueueAutomaticPopulator.PopulateQueues("5", 0).ToList();
                QueueAutomaticPopulator.PopulateQueues("6", 110).ToList();
                QueueAutomaticPopulator.PopulateQueues("7", 15).ToList();
                QueueAutomaticPopulator.PopulateQueues("8", 8).ToList();
                QueueAutomaticPopulator.PopulateQueues("9", 210).ToList();
                QueueAutomaticPopulator.PopulateQueues("10", 10).ToList();
            }
        }
    }

    public static class QueueAutomaticPopulator
    {
        public static IEnumerable<Models.Queue> PopulateQueues(string activityId, int numberOfPersonsInQueue)
        {
            var queueData = new QueueData();

            var models = new List<Models.Queue>();
            for (int i = 1; i < numberOfPersonsInQueue; i++)
            {
                var queue = new Queue("NOR", "KRSDYREPARK", activityId, (1000 + i).ToString());
                queueData.Add(queue);
            }

            return models;
        }
    }
}

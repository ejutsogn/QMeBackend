using Bumbleberry.QMeService.Biz;
using Bumbleberry.QMeService.Data;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Models = Bumbleberry.QMeService.Models;

namespace QMeServiceTests
{
    public class QueueBizTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase(100, 70, 50, 30, 200, 15, "act5")]
        [TestCase(100, 0, 0, 0, 20, 50, "act1")]
        public void GetActivityWithMaxQueue(int nrQ1, int nrQ2, int nrQ3, int nrQ4, int nrQ5, int nrQ6, string expectedActivityIdLongestQueueTime)
        {
            var queueDataMock = new Mock<IQueueData>();
            var queue = QueueDataTestHelper.GetActivityQueues("act1", nrQ1).ToList();
            var queue2 = QueueDataTestHelper.GetActivityQueues("act2", nrQ2);
            var queue3 = QueueDataTestHelper.GetActivityQueues("act3", nrQ3);
            var queue4 = QueueDataTestHelper.GetActivityQueues("act4", nrQ4);
            var queue5 = QueueDataTestHelper.GetActivityQueues("act5", nrQ5);
            var queue6 = QueueDataTestHelper.GetActivityQueues("act6", nrQ6);

            queue.AddRange(queue2);
            queue.AddRange(queue3);
            queue.AddRange(queue4);
            queue.AddRange(queue5);
            queue.AddRange(queue6);

            //queueDataMock.Setup(x => x.GetActivityQueues("1", "1")).Returns(queue);
            var queueBizInherited = new QueueBizInherited(queueDataMock.Object);

            var actualIdWithMaxQueue = queueBizInherited.GetActivityIdWithMaxQueueNumbers(queue);

            Assert.AreEqual(expectedActivityIdLongestQueueTime, actualIdWithMaxQueue, $"Expected id: {expectedActivityIdLongestQueueTime} -  Actual id: {actualIdWithMaxQueue}");
        }

        [TestCase(100, 70, 50, 30, 200, 15, "act5")]
        [TestCase(100, 0, 0, 0, 20, 50, "act1")]
        [TestCase(102, 98, 0, 0, 20, 50, "act2")]
        public void GetActivityQueueInfosWithLongestProgressbar(int nrQ1, int nrQ2, int nrQ3, int nrQ4, int nrQ5, int nrQ6, string expectedActivityIdLongestProgressbar)
        {
            var countryId = "1";
            var companyGuid = "1";
            var userGuid = "12345";
            var queueDataMock = new Mock<IQueueData>();
            var queue = QueueDataTestHelper.GetActivityQueues("act1", nrQ1).ToList();
            var queue2 = QueueDataTestHelper.GetActivityQueues("act2", nrQ2);
            var queue3 = QueueDataTestHelper.GetActivityQueues("act3", nrQ3);
            var queue4 = QueueDataTestHelper.GetActivityQueues("act4", nrQ4);
            var queue5 = QueueDataTestHelper.GetActivityQueues("act5", nrQ5);
            var queue6 = QueueDataTestHelper.GetActivityQueues("act6", nrQ6);

            queue.AddRange(queue2);
            queue.AddRange(queue3);
            queue.AddRange(queue4);
            queue.AddRange(queue5);
            queue.AddRange(queue6);

            queueDataMock.Setup(x => x.GetActivityQueues(countryId, companyGuid, "", "")).Returns(queue);
            queueDataMock.Setup(x => x.GetActivityQueues(countryId, companyGuid, It.IsAny<string>(), userGuid)).Returns(queue2);
            var queueBizInherited = new QueueBizInherited(queueDataMock.Object);

            var queueInfos = queueBizInherited.GetActivityQueueInfos(countryId, companyGuid, userGuid);
            var queueInfo = queueInfos
                                    .OrderByDescending(x => x.ProgressBarInPercent)
                                    .FirstOrDefault();

            var actualIdWithLongestProgressbar = queueInfo.ActitityGuid;

            Assert.AreEqual(expectedActivityIdLongestProgressbar, actualIdWithLongestProgressbar, $"Expected id: {expectedActivityIdLongestProgressbar} -  Actual id: {actualIdWithLongestProgressbar}");
        }
    }

    public class QueueBizInherited : QueueBiz
    {
        public QueueBizInherited(IQueueData queueData) : base(queueData) { }

        public new string GetActivityIdWithMaxQueueNumbers(IEnumerable<Models.Queue> activityQueues)
        {
            return base.GetActivityIdWithMaxQueueNumbers(activityQueues);
        }

        public new IEnumerable<Models.QueueInfo> GetActivityQueueInfos(string countryId, string companyId, string userId)
        {
            return base.GetQueueInfos(countryId, companyId, userId);
        }
    }

    public static class QueueDataTestHelper
    {
        public static IEnumerable<Models.Queue> GetActivityQueues(string activityGuid, int nrOfQueueItems)
        {
            var countryId = "NOR";
            var companyGuid = "KRSDYREPARK";
            var models = new List<Models.Queue>();
            for (int i = 1; i <= nrOfQueueItems; i++)
            {
                models.Add(GetNewQueueItem(countryId, companyGuid, activityGuid, (100+i).ToString(), i));
            }

            return models;
        }

        private static Models.Queue GetNewQueueItem(string countryId, string companyGuid, string actitityGuid, string userGuid, int nrInQueue)
        {
            return new Models.Queue(countryId, companyGuid, actitityGuid, userGuid) { QueueTime = DateTime.Now.AddSeconds(nrInQueue*3) };
        }
    }
}

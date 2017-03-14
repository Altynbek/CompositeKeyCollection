using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KeyCollectionTest.Classes;

namespace CompositeKeyTest
{
    [TestClass]
    public class CompositeKeyCollectionTest
    {
        [TestMethod]
        public void AddingItemsToCompositeKeyCollectionTest()
        {
            DateTime todayDt = DateTime.Today;
            var taskDoneCollection = new CompositeKeyCollection<UserType, string, int>();
            taskDoneCollection.Add(new UserType(todayDt), "Developers", 10);
            taskDoneCollection.Add(new UserType(todayDt), "Managers", 12);
            taskDoneCollection.Add(new UserType(todayDt), "Analiytics", 15);
            taskDoneCollection.Add(new UserType(todayDt.AddDays(1)), "Developers", 12);
            taskDoneCollection.Add(new UserType(todayDt.AddDays(1)), "Managers", 10);
            taskDoneCollection.Add(new UserType(todayDt.AddDays(1)), "Analiytics", 4);

            const int alreadyAddedRecords = 6;
            Assert.AreEqual(alreadyAddedRecords, taskDoneCollection.Count);
        }

        [TestMethod]
        public void RemovingItemsFromCompositeKeyCollectionTest()
        {
            DateTime todayDt = DateTime.Today;
            var taskDoneCollection = new CompositeKeyCollection<UserType, string, int>();

            taskDoneCollection.Add(new UserType(todayDt), "Developers", 10);
            taskDoneCollection.Add(new UserType(todayDt), "Managers", 12);
            taskDoneCollection.Add(new UserType(todayDt), "Analiytics", 15);

            taskDoneCollection.Remove(new UserType(todayDt), "Developers");
            taskDoneCollection.Remove(new UserType(todayDt), "Analiytics");

            const int expectedRecordsCount = 1;
            int actualRecordsCount = taskDoneCollection.Count;
            Assert.AreEqual(expectedRecordsCount, actualRecordsCount);

            bool keyExist = taskDoneCollection.ContainsKey(new UserType(todayDt), "Managers");
            Assert.IsTrue(keyExist);

        }

        [TestMethod]
        public void FindRecordsByCompositeKeyTest()
        {
            DateTime dt1 = new DateTime(2017, 03, 13);
            DateTime dt2 = new DateTime(2017, 03, 14);

            var taskDoneCollection = new CompositeKeyCollection<UserType, string, int>();
            taskDoneCollection.Add(new UserType(dt1), "Developers", 10);
            taskDoneCollection.Add(new UserType(dt1), "Managers", 12);
            taskDoneCollection.Add(new UserType(dt1), "Analiytics", 15);
            taskDoneCollection.Add(new UserType(dt2), "Developers", 12);
            taskDoneCollection.Add(new UserType(dt2), "Managers", 10);
            taskDoneCollection.Add(new UserType(dt2), "Analiytics", 4);

            var key1 = new UserType(dt1);
            var key2 = "Developers";
            var pairs = taskDoneCollection.Find(key1, key2);
            int expectedRecordsCount = 1;
            Assert.AreEqual(expectedRecordsCount, pairs.Count, "Just one key value pair was expected");

            pairs = taskDoneCollection.Find(key1, null);
            expectedRecordsCount = 3;
            Assert.AreEqual(expectedRecordsCount, pairs.Count, "Three records was expected");

            pairs = taskDoneCollection.Find(null, key2);
            expectedRecordsCount = 2;
            Assert.AreEqual(expectedRecordsCount, pairs.Count, "Only two records was expected");
        }

        [TestMethod]
        public void ContainsKeyAndValuesTest()
        {
            DateTime dt1 = new DateTime(2017, 03, 13);
            DateTime dt2 = new DateTime(2017, 03, 14);

            var taskDoneCollection = new CompositeKeyCollection<UserType, string, int>();
            taskDoneCollection.Add(new UserType(dt1), "Developers", 10);
            taskDoneCollection.Add(new UserType(dt1), "Managers", 12);

            bool containsKey = taskDoneCollection.ContainsKey(new UserType(dt1), "Managers");
            bool containsValue = taskDoneCollection.ContainsValue(12);
            Assert.IsTrue(containsKey);
            Assert.IsTrue(containsValue);

            bool NotExistedKey = taskDoneCollection.ContainsKey(new UserType(dt1), "Analytics");
            bool NotExistedValue = taskDoneCollection.ContainsValue(15);
            Assert.IsFalse(NotExistedKey);
            Assert.IsFalse(NotExistedValue);
        }

        [TestMethod]
        public void ClearCollectionTest()
        {
            DateTime dt1 = new DateTime(2017, 03, 13);
            var taskDoneCollection = new CompositeKeyCollection<UserType, string, int>();
            taskDoneCollection.Add(new UserType(dt1), "Developers", 10);
            taskDoneCollection.Add(new UserType(dt1), "Managers", 12);

            int currentCount = taskDoneCollection.Count;
            int expectedCount = 0;
            Assert.AreNotEqual(expectedCount, currentCount);

            taskDoneCollection.Clear();
            Assert.AreEqual(expectedCount, taskDoneCollection.Count);
        }
    }
}

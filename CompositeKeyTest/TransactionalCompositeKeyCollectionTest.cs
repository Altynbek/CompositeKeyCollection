using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KeyCollectionTest.POC;
using KeyCollectionTest.Classes;

namespace CompositeKeyTest
{
    [TestClass]
    public class TransactionalCompositeKeyCollectionTest
    {
        [TestMethod]
        public void PerformTransactionsTest()
        {
            var collection = new TransactionalDictionary<UserType, string, int>();
            collection.Add(new UserType(DateTime.Now), null, 10);

            int expectedRecordsCount = 1;
            int actualRecordCount = collection.Count;
            Assert.AreNotEqual(expectedRecordsCount, actualRecordCount);

            var key1 = new UserType(DateTime.Today);
            var key2 = "Analytics";
            collection.Add(key1, key2, 20);
            actualRecordCount = collection.Count;
            Assert.AreEqual(expectedRecordsCount, actualRecordCount);

            collection.Remove(key1, "Managers");
            actualRecordCount = collection.Count;
            expectedRecordsCount = 1;
            Assert.AreEqual(expectedRecordsCount, actualRecordCount);
        }
    }
}

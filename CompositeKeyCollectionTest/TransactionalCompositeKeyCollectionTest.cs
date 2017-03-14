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
        [ExpectedException(typeof(ArgumentNullException), "Passed composite key arguments can't be null")]
        public void RollBackTransactionWhenExceptionOccured()
        {
            var collection = new TransactionalCompositeKeyCollection<UserType, string, int>();
            collection.Add(new UserType(DateTime.Now), null, 10);
        }

        [TestMethod]
        public void DontBreakInternalDictionaryStateWhenExceptionOccured()
        {
            var collection = new TransactionalCompositeKeyCollection<UserType, string, int>();
            collection.Add(new UserType(DateTime.Today), "Managers", 10);
            collection.Add(new UserType(DateTime.Today), "Analitics", 15);
            int expectedRecordsCount = 2;
            Assert.AreEqual(expectedRecordsCount, collection.Count);

            // adding an invalid object to the collection
            try
            {
                collection.Add(null, "Developers", 20);
            }
            catch (ArgumentNullException ex) { }
            Assert.AreEqual(expectedRecordsCount, collection.Count);

            collection.Add(new UserType(DateTime.Today), "Developers", 20);
            expectedRecordsCount = 3;
            Assert.AreEqual(expectedRecordsCount, collection.Count);
        }

    }
}

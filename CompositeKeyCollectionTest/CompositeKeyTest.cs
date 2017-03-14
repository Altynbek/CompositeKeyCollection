using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KeyCollectionTest.Classes;

namespace CompositeKeyTest
{
    [TestClass]
    public class CompositeKeyTest
    {
        [TestMethod]
        public void CompositeKeysEqualWhenKeysPropertiesEqual()
        {
            DateTime todayDt = DateTime.Today;
            var key1= new CompositeKey<UserType, string>(new UserType(todayDt), "Developers");
            var key2 = new CompositeKey<UserType, string>(new UserType(todayDt), "Developers");

            if(!key1.Equals(key2) || key1 != key2)
                Assert.Fail("Keys are NOT equal!");
        }


        [TestMethod]
        public void CompositeKeysNotEqualWhenKeysPropertiesNotEqual()
        {
            DateTime dt1 = new DateTime(2017, 03, 13);
            DateTime dt2 = new DateTime(2017, 03, 14);
            var key1 = new CompositeKey<UserType, string>(new UserType(dt1), "Developers");
            var key2 = new CompositeKey<UserType, string>(new UserType(dt2), "Managers");

            if (key1.Equals(key2) || key1 == key2)
                Assert.Fail("Keys should not be equal");
        }
    }
}

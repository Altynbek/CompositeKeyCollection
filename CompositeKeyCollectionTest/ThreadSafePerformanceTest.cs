using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KeyCollectionTest.Classes;
using System.Diagnostics;

namespace CompositeKeyTest
{
    [TestClass]
    public class ThreadSafePerformanceTest
    {

        private ThreadSafeCompositeKeyCollection<UserType, string, int> _collection = new ThreadSafeCompositeKeyCollection<UserType, string, int>();
        [TestMethod]
        public void FindMehodPerformanceTest()
        {
            Random rnd = new Random();
            DateTime dt = new DateTime(1800, 01, 01, 04, 50, 32);

            UserType key1 = new UserType(dt.AddDays(55999));
            string key2 = "Developers";
            for (int idx = 0; idx < 100000; idx++)
                _collection.Add(new UserType(dt.AddDays(-idx)), key2, rnd.Next(1, 20));

            var timeElapsed = CountSearchTime(key1, key2);


            DateTime dt2 = new DateTime(2000, 01, 01, 07, 09, 25);
            key2 = "Managers";
            for (int idx = 0; idx < 100000; idx++)
                _collection.Add(new UserType(dt2.AddDays(-idx)), key2, rnd.Next(1, 20));

            var timeElapsed2 = CountSearchTime(key1, key2);


            key2 = "Testers";
            DateTime dt3 = new DateTime(2200, 01, 01, 4, 11, 10);
            for (int idx = 0; idx < 100000; idx++)
                _collection.Add(new UserType(dt3.AddDays(-idx)), key2, rnd.Next(1, 20));
            
            var timeElapsed3 = CountSearchTime(key1, key2);
        }

        private static TimeSpan CountSearchTime(UserType key1, string key2)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            sw.Stop();
            var time = sw.Elapsed;
            return time;
        }
    }
}

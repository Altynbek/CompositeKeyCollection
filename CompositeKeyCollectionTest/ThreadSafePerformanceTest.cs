using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KeyCollectionTest.Classes;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace CompositeKeyTest
{
    [TestClass]
    public class ThreadSafePerformanceTest
    {
        private static ThreadSafeCompositeKeyCollection<int, DateTime, string> _thSafeCollection;
        private static Dictionary<KeyValuePair<int, DateTime>, string> _kvpDictionary;

        [TestMethod]
        public void LinqComparisonTest()
        {
            _thSafeCollection = new ThreadSafeCompositeKeyCollection<int, DateTime, string>();
            _kvpDictionary = new Dictionary<KeyValuePair<int, DateTime>, string>();

            Random rnd = new Random();
            DateTime dt = DateTime.Today;

            string value = "Developers";
            for (int idx = 0; idx < 100000; idx++)
            {
                var datetime = dt.AddDays(-idx);
                _kvpDictionary.Add(new KeyValuePair<int, DateTime>(idx, datetime), value);
                _thSafeCollection.Add(idx, datetime, value);
            }

            int key1 = 55000;
            DateTime key2 = DateTime.Today.AddDays(-55000);
            var customCollectionSearchTime = CountSearchTimeOnCustomCollection(key1, key2);
            var dictionaryCollectionSearchTime = CountSearchTimeOnDictionary(key1, key2);


            dt = new DateTime(1900, 01, 01);
            for (int idx = 0; idx < 100000; idx++)
            {
                var datetime = dt.AddDays(idx);
                value = "Managers";
                _kvpDictionary.Add(new KeyValuePair<int, DateTime>(idx, datetime), value);
                _thSafeCollection.Add(idx, datetime, value);
            }

            var customCollectionSearchTime2 = CountSearchTimeOnCustomCollection(key1, key2);
            var dictionaryCollectionSearchTime2 = CountSearchTimeOnDictionary(key1, key2);


            dt = new DateTime(1500, 01, 01);
            for (int idx = 0; idx < 100000; idx++)
            {
                var datetime = dt.AddMinutes(idx);
                value = "Testers";
                _kvpDictionary.Add(new KeyValuePair<int, DateTime>(idx, datetime), value);
                _thSafeCollection.Add(idx, datetime, value);
            }

            var customCollectionSearchTime3 = CountSearchTimeOnCustomCollection(key1, key2);
            var dictionaryCollectionSearchTime3 = CountSearchTimeOnDictionary(key1, key2);

            dt = new DateTime(2200, 01, 01);
            for (int idx = 0; idx < 100000; idx++)
            {
                var datetime = dt.AddMinutes(idx);
                value = "Analytics";
                _kvpDictionary.Add(new KeyValuePair<int, DateTime>(idx, datetime), value);
                _thSafeCollection.Add(idx, datetime, value);
            }

            var customCollectionSearchTime4 = CountSearchTimeOnCustomCollection(key1, key2);
            var dictionaryCollectionSearchTime4 = CountSearchTimeOnDictionary(key1, key2);

            Debug.WriteLine("customCollectionSearchTime = {0}", customCollectionSearchTime);
            Debug.WriteLine("dictionaryCollectionSearchTime = {0}", dictionaryCollectionSearchTime);
            Debug.WriteLine("customCollectionSearchTime2 = {0}", customCollectionSearchTime2);
            Debug.WriteLine("dictionaryCollectionSearchTime2 = {0}", dictionaryCollectionSearchTime2);
            Debug.WriteLine("customCollectionSearchTime3 = {0}", customCollectionSearchTime3);
            Debug.WriteLine("dictionaryCollectionSearchTime3 = {0}", dictionaryCollectionSearchTime3);
            Debug.WriteLine("customCollectionSearchTime4 = {0}", customCollectionSearchTime4);
            Debug.WriteLine("dictionaryCollectionSearchTime4 = {0}", dictionaryCollectionSearchTime4);
        }


        private static TimeSpan CountSearchTimeOnCustomCollection(int key1, DateTime key2)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var items = _thSafeCollection.Find(key1, key2);
            sw.Stop();
            var time = sw.Elapsed;
            return time;
        }

        private static TimeSpan CountSearchTimeOnDictionary(int key1, DateTime key2)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var item = _kvpDictionary.FirstOrDefault(x => x.Key.Key == key1 && x.Key.Value == key2);
            sw.Stop();
            var time = sw.Elapsed;
            return time;
        }
    }
}

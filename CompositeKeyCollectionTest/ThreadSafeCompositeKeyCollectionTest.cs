using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KeyCollectionTest.Classes;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

namespace CompositeKeyTest
{
    [TestClass]
    public class ThreadSafeCompositeKeyCollectionTest
    {
        private static ThreadSafeCompositeKeyCollection<UserType, string, int> _concurentCollection;

        [TestMethod]
        public void UpdateCollectionConcurentlyTest()
        {
            _concurentCollection = new ThreadSafeCompositeKeyCollection<UserType, string, int>();

            Task.Run(() => RunFirstWriteThread());
            Task.Run(() => RunSecondWriteThread());
            Task.Run(() => RunThirdWriteThread());
            Task.Run(() => RunFirstReadThread());
            Task.Run(() => RunSecondReadThread());
            Task.Run(() => RunFirstRemovingThread());
        }

        private void RunFirstWriteThread()
        {
            const int maxIterations = 1000;
            DateTime dt = new DateTime(2017, 12, 31);
            Random rnd = new Random();

            for (int idx = 0; idx < maxIterations; idx++)
            {
                UserType key1 = new UserType(dt.AddDays(-idx));
                const string key2 = "Developers";

                _concurentCollection.Add(key1, key2, rnd.Next(1, 15));
            }
        }

        private void RunSecondWriteThread()
        {
            const int maxIterations = 3000;
            DateTime dt = new DateTime(2017, 12, 31);
            Random rnd = new Random();

            for (int idx = 0; idx < maxIterations; idx++)
            {
                UserType key1 = new UserType(dt.AddDays(-idx));
                const string key2 = "Managers";

                _concurentCollection.Add(key1, key2, rnd.Next(1, 15));
            }
        }

        private void RunThirdWriteThread()
        {
            const int maxIterations = 3000;
            DateTime dt = new DateTime(2017, 12, 31);
            Random rnd = new Random();

            for (int idx = 0; idx < maxIterations; idx++)
            {
                UserType key1 = new UserType(dt.AddDays(-idx));
                const string key2 = "Analytics";

                _concurentCollection.Add(key1, key2, rnd.Next(1, 15));
            }
        }

        private void RunFirstRemovingThread()
        {
            const int maxIterationsCount = 500;
            for (int idx = 0; idx < maxIterationsCount; idx++)
            {
                var keys = _concurentCollection.Keys;
                if (keys != null && keys.Count > 0)
                {
                    var removedKey = keys.First();
                    _concurentCollection.Remove(removedKey.Id, removedKey.Name);
                }
            }
        }
        

        private void RunFirstReadThread()
        {
            for (int idx = 0; idx < 500; idx++)
            {
                int keysCount = _concurentCollection.Values.Count;
            }
        }

        private void RunSecondReadThread()
        {
            for (int idx = 0; idx < 500; idx++)
            {
                int keysCount = _concurentCollection.Keys.Count;
            }
        }
    }
}

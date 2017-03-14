using KeyCollectionTest.Classes;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Transactions;
using KeyCollectionTest.POC;
using System.Linq;

namespace KeyCollectionTest
{
    class Program
    {
        private static ThreadSafeCompositeKeyCollection<UserType, string, int> _collection2 = null;
        static void Main(string[] args)
        {
            // В проекте CompositeKeyCollection имеется 3 реализации класса коллекций с композитными (составными) ключами:
            // 1. CompositeKeyCollection<TKey1, Tkey2, TData> - обычная обобщенная реализация класса коллекции (не потокобезопасная)
            // 2. ThreadSafeCompositeKeyCollection<Tkey1, Tkey2, TData> - потокобезопасная реализация класса CompositeKeyCollection<TKey1, TKey2, TData>
            // 3. TransactionalCompositeKeyCollection<TKey1, TKey2, TData> - класс коллекции с комопозитными ключами и поддержкой транзакционности на операции вставки и удаления записей в коллекцию (PS.: пока еще не потокобезопасная)

            // Примеры использования коллекций:
            // 1. CompositeKeyCollection<TKey1, Tkey2, TData> 
            var currentDate = DateTime.Today;
            var tasksDoneCollection1 = new CompositeKeyCollection<UserType, string, int>();
            tasksDoneCollection1.Add(new UserType(currentDate), "Developers", 10);
            tasksDoneCollection1.Add(new UserType(currentDate), "Managers", 12);
            tasksDoneCollection1.Add(new UserType(currentDate.AddDays(1)), "Developers", 7);
            PrintCollection(tasksDoneCollection1);

            var key1 = new UserType(currentDate);
            var key2 = "Developers";
            tasksDoneCollection1.Remove(key1, key2);
            PrintCollection(tasksDoneCollection1);

            key1 = new UserType(currentDate.AddDays(1));
            var items = tasksDoneCollection1.Find(key1, key2);
            Console.WriteLine("Items found: {0}", items.Count);

            var value = tasksDoneCollection1[key1, key2];
            Console.WriteLine("Value in dictionary: {0}", value);

            tasksDoneCollection1.Clear();


            // 2. ThreadSafeCompositeKeyCollection<Tkey1, Tkey2, TData> 
            _collection2 = new ThreadSafeCompositeKeyCollection<UserType, string, int>();
            Task.Run(() => RunFirstWriteThread());
            Task.Run(() => RunSecondWriteThread());
            Task.Run(() => RunShowKeyCountThread());


            // 3.TransactionalDictionary<TKey1, TKey2, TData> 
            var collection3 = new TransactionalCompositeKeyCollection<UserType, string, int>();
            collection3.Add(new UserType(DateTime.Today), "Managers", 10);
            collection3.Add(new UserType(DateTime.Today), "Analitics", 15);
            
            // adding an invalid object to the collection
            try
            {
                collection3.Add(null, "Developers", 20);
            }
            catch (ArgumentNullException ex) { }

            Console.Read();
        }

        private static void RunShowKeyCountThread()
        {
            for (int idx = 0; idx < 30; idx++)
            {
                Console.WriteLine("At this moment collection contains {0} keys ", _collection2.Keys.Count);
            }
        }

        private static void PrintCollection<TKey1, TKey2, TData>(CompositeKeyCollection<TKey1, TKey2, TData> collection)
        {
            for (int idx = 0; idx < collection.Count; idx++)
            {
                Console.WriteLine(collection[idx]);
            }
            Console.WriteLine(Environment.NewLine);
        }

        private static void RunFirstWriteThread()
        {
            const int maxIterations = 3000;
            DateTime dt = new DateTime(2017, 12, 31);
            Random rnd = new Random();

            for (int idx = 0; idx < maxIterations; idx++)
            {
                UserType key1 = new UserType(dt.AddDays(-idx));
                const string key2 = "Developers";

                _collection2.Add(key1, key2, rnd.Next(1, 15));
            }
        }

        private static void RunSecondWriteThread()
        {
            const int maxIterations = 3000;
            DateTime dt = new DateTime(2017, 12, 31);
            Random rnd = new Random();

            for (int idx = 0; idx < maxIterations; idx++)
            {
                UserType key1 = new UserType(dt.AddDays(-idx));
                const string key2 = "Managers";

                _collection2.Add(key1, key2, rnd.Next(1, 15));
            }
        }
    }
}
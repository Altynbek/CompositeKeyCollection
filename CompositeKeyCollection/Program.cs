using KeyCollectionTest.Classes;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Transactions;
using KeyCollectionTest.POC;

namespace KeyCollectionTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // В проекте CompositeKeyCollection имеется 3 реализации класса коллекций с композитными (сотставными) ключами:
            // 1. CompositeKeyCollection<TKey1, Tkey2, TData> - обычная обобщенная реализация класс коллекции (не потоко безопасная)
            // 2. ThreadSafeCompositeKeyCollection<Tkey1, Tkey2, TData> - потокобезопасная реализация класса CompositeKeyCollection<TKey1, TKey2, TData>
            // 3. TransactionalDictionary<TKey1, TKey2, TData>
            // 
            Console.Read();
        }
    }
}

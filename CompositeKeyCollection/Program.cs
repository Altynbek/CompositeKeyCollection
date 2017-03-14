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
            var collection = new TransactionalDictionary<UserType, string, int>();
            collection.Add(new UserType(DateTime.Now), "qwe", 10);


            Console.Read();
        }
    }
}

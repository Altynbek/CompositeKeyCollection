using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace KeyCollectionTest.Classes
{
    public class ThreadSafeCompositeKeyCollection<Tkey1, Tkey2, TData> : CompositeKeyCollection<Tkey1, Tkey2, TData>
        where Tkey1 : class
        where Tkey2 : class
    {
        private ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim();


        public override void Add(Tkey1 key1, Tkey2 key2, TData data)
        {
            rwLock.EnterWriteLock();
            try
            {
                base.Add(key1, key2, data);
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }


        public override void Remove(Tkey1 key1, Tkey2 key2)
        {
            rwLock.EnterWriteLock();
            try
            {
                base.Remove(key1, key2);
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }


        public override void Clear()
        {
            rwLock.EnterWriteLock();
            try
            {
                base.Clear();
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }


        public override bool ContainsKey(Tkey1 key1, Tkey2 key2)
        {
            rwLock.EnterReadLock();
            try
            {
                return base.ContainsKey(key1, key2);
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }


        public override bool ContainsValue(TData data)
        {
            rwLock.EnterReadLock();
            try
            {
                return base.ContainsValue(data);
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public override Dictionary<CompositeKey<Tkey1, Tkey2>, TData>.KeyCollection Keys
        {
            get
            {
                try
                {
                    rwLock.EnterReadLock();
                    return base.Keys;
                }
                finally
                {
                    rwLock.ExitReadLock();
                }
            }
        }

        public override Dictionary<CompositeKey<Tkey1, Tkey2>, TData>.ValueCollection Values
        {
            get
            {
                try
                {
                    rwLock.EnterReadLock();
                    return base.Values;
                }
                finally
                {
                    rwLock.ExitReadLock();
                }
            }
        }

        public override KeyValuePair<CompositeKey<Tkey1, Tkey2>, TData> this[int index]
        {
            get
            {
                rwLock.EnterReadLock();
                try
                {
                    return base[index];
                }
                finally
                {
                    rwLock.ExitReadLock();
                }
            }
        }

        public override TData this[Tkey1 key1, Tkey2 key2]
        {
            get
            {
                rwLock.EnterReadLock();
                try
                {
                    return base[key1, key2];
                }
                finally
                {
                    rwLock.ExitReadLock();
                }
            }

            set
            {
                rwLock.EnterWriteLock();
                try
                {
                    base[key1, key2] = value;
                }
                finally
                {
                    rwLock.ExitWriteLock();
                }
            }
        }
    }
}

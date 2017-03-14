using KeyCollectionTest.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace KeyCollectionTest.POC
{
    public class CompositeKeyCollection2<TKey1, TKey2, TData> : IEnlistmentNotification
        where TKey1 : class
        where TKey2 : class
    {
        #region Fields
        protected Dictionary<CompositeKey<TKey1, TKey2>, TData> _dictionary;
        protected Dictionary<TKey1, Dictionary<TKey2, TData>> _idDictionary;
        protected Dictionary<TKey2, Dictionary<TKey1, TData>> _namedDictionary;

        protected KeyValuePair<CompositeKey<TKey1, TKey2>, TData>? _tempAddedObj = null;
        protected KeyValuePair<CompositeKey<TKey1, TKey2>, TData>? _tempRemovedObj = null;
        #endregion


        #region Constructors
        public CompositeKeyCollection2()
        {
            _dictionary = new Dictionary<CompositeKey<TKey1, TKey2>, TData>();
            _idDictionary = new Dictionary<TKey1, Dictionary<TKey2, TData>>();
            _namedDictionary = new Dictionary<TKey2, Dictionary<TKey1, TData>>();
        }
        #endregion


        #region Public methods
        public virtual void Add(TKey1 key1, TKey2 key2, TData data)
        {
            var key = new CompositeKey<TKey1, TKey2>(key1, key2);
            _tempAddedObj = new KeyValuePair<CompositeKey<TKey1, TKey2>, TData>(key, data);
            Add(key, data);
        }


        public virtual void Remove(TKey1 key1, TKey2 key2)
        {
            var compositeKey = new CompositeKey<TKey1, TKey2>(key1, key2);
            if (_dictionary.ContainsKey(compositeKey))
            {
                var data = _dictionary[compositeKey];
                _tempRemovedObj = new KeyValuePair<CompositeKey<TKey1, TKey2>, TData>(compositeKey, data);
                Remove(compositeKey);
            }
        }

        public virtual bool ContainsKey(TKey1 key1, TKey2 key2)
        {
            CheckKeyParameters(key1, key2);
            var key = new CompositeKey<TKey1, TKey2>(key1, key2);

            bool contains = _dictionary.ContainsKey(key);
            return contains;
        }

        public virtual bool ContainsValue(TData data)
        {
            bool contais = _dictionary.ContainsValue(data);
            return contais;
        }

        public virtual void Clear()
        {
            _dictionary.Clear();
            _idDictionary.Clear();
            _namedDictionary.Clear();
        }

        public virtual TData this[TKey1 key1, TKey2 key2]
        {
            get
            {
                if (key1 == null && key2 == null)
                    throw new ArgumentNullException("key1 and key2", "Key should not be nullable");

                var key = new CompositeKey<TKey1, TKey2>(key1, key2);
                return _dictionary[key];
            }
            set
            {
                CheckKeyParameters(key1, key2);
                var key = new CompositeKey<TKey1, TKey2>(key1, key2);
                SetValue(key, value);
            }
        }
        public virtual KeyValuePair<CompositeKey<TKey1, TKey2>, TData> this[int index]
        {
            get
            {
                return _dictionary.ElementAt(index);
            }
        }

        public virtual Dictionary<CompositeKey<TKey1, TKey2>, TData> Find(TKey1 key1, TKey2 key2)
        {
            var result = new Dictionary<CompositeKey<TKey1, TKey2>, TData>();

            if (key1 != null && key2 != null)                // получаем элемент из исходной коллекции по композитному ключу
            {
                var key = new CompositeKey<TKey1, TKey2>(key1, key2);
                result.Add(key, _dictionary[key]);
            }
            else if (key1 != null)
            {
                var items = _idDictionary[key1];            // получаем по дате словарь из пар <name, value>
                for (int idx = 0; idx < items.Count; idx++)
                {
                    var currentPair = items.ElementAt(idx);
                    result.Add(new CompositeKey<TKey1, TKey2>(key1, currentPair.Key), currentPair.Value);
                }
            }
            else if (key2 != null)                           // получаем по имени словарь из пар <id, value>
            {
                var items = _namedDictionary[key2];
                for (int idx = 0; idx < items.Count; idx++)
                {
                    var currentPair = items.ElementAt(idx);
                    result.Add(new CompositeKey<TKey1, TKey2>(currentPair.Key, key2), currentPair.Value);
                }
            }

            return result;
        }


        #endregion


        #region Properties
        public virtual Dictionary<CompositeKey<TKey1, TKey2>, TData>.KeyCollection Keys
        {
            get
            {
                var lst = _dictionary.Keys;
                return lst;
            }
        }

        public virtual Dictionary<CompositeKey<TKey1, TKey2>, TData>.ValueCollection Values
        {
            get
            {
                var lst = _dictionary.Values;
                return lst;
            }
        }

        public virtual int Count
        {
            get { return _dictionary.Count; }
        }
        #endregion


        #region Private methods
        protected void CheckKeyParameters(TKey1 key1, TKey2 key2)
        {
            if (key1 == null)
                throw new ArgumentNullException("key1");

            if (key2 == null)
                throw new ArgumentNullException("key2");
        }

        private void SetValue(CompositeKey<TKey1, TKey2> key, TData data)
        {
            _dictionary.Add(key, data);
        }


        private void Add(CompositeKey<TKey1, TKey2> key, TData data)
        {
            Console.WriteLine("Add method invoked");
            CheckKeyParameters(key.Id, key.Name);

            Transaction currentTx = Transaction.Current;
            if (currentTx != null)
                currentTx.EnlistVolatile(this, EnlistmentOptions.None);

            try
            {
                _dictionary.Add(key, data);

                // add to id collection
                if (_idDictionary.ContainsKey(key.Id))
                {
                    var dictionaryWithNames = _idDictionary[key.Id];
                    dictionaryWithNames.Add(key.Name, data);
                }
                else
                {
                    var dictionaryWithNames = new Dictionary<TKey2, TData>();
                    dictionaryWithNames.Add(key.Name, data);
                    _idDictionary[key.Id] = dictionaryWithNames;
                }


                // add to named collection
                if (_namedDictionary.ContainsKey(key.Name))
                {
                    var dictionaryWithId = _namedDictionary[key.Name];
                    dictionaryWithId.Add(key.Id, data);
                }
                else
                {
                    var dictionaryWithId = new Dictionary<TKey1, TData>();
                    dictionaryWithId.Add(key.Id, data);
                    _namedDictionary[key.Name] = dictionaryWithId;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("exception occured");

            }
        }


        private void Remove(CompositeKey<TKey1, TKey2> key)
        {
            CheckKeyParameters(key.Id, key.Name);
            if (_dictionary.ContainsKey(key))
                _dictionary.Remove(key);

            // remove from _idDictionary
            if (_idDictionary.ContainsKey(key.Id))
            {
                var dictionaryWithNames = _idDictionary[key.Id];
                if (dictionaryWithNames.ContainsKey(key.Name))
                    dictionaryWithNames.Remove(key.Name);

                if (dictionaryWithNames.Count == 0)
                    _idDictionary.Remove(key.Id);
            }

            // remove from _namedDictionary
            if (_namedDictionary.ContainsKey(key.Name))
            {
                var dictionaryWithId = _namedDictionary[key.Name];
                if (dictionaryWithId.ContainsKey(key.Id))
                    dictionaryWithId.Remove(key.Id);

                if (dictionaryWithId.Count == 0)
                    _namedDictionary.Remove(key.Name);
            }
        }

        #endregion


        #region Interface implementation
        public void Commit(Enlistment enlistment)
        {
            enlistment.Done();
            // save state
        }

        public void InDoubt(Enlistment enlistment)
        {
        }

        public void Prepare(PreparingEnlistment preparingEnlistment)
        {
            //preparingEnlistment.Prepared();
            preparingEnlistment.ForceRollback();
        }

        public void Rollback(Enlistment enlistment)
        {
            Console.WriteLine("rollback was invoked");
            if (_tempAddedObj != null)
            {
                var kvp = (KeyValuePair<CompositeKey<TKey1, TKey2>, TData>)_tempAddedObj;
                Remove(kvp.Key);
                _tempRemovedObj = null;
            }

            if (_tempRemovedObj != null)
            {
                var kvp = (KeyValuePair<CompositeKey<TKey1, TKey2>, TData>)_tempRemovedObj;
                Add(kvp.Key, kvp.Value);
                _tempRemovedObj = null;
            }
        }
        #endregion
    }

    public class TransactionalDictionary<TKey1, TKey2, TData>
        where TKey1 : class
        where TKey2 : class
    {
        private CompositeKeyCollection2<TKey1, TKey2, TData> _compositeKeyCollection;

        public TransactionalDictionary()
        {
            _compositeKeyCollection = new CompositeKeyCollection2<TKey1, TKey2, TData>();
        }

        public virtual void Add(TKey1 key1, TKey2 key2, TData data)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    _compositeKeyCollection.Add(key1, key2, data);
                    scope.Complete();
                }
            }
            catch(Exception ex)
            {
            }
        }

        public virtual void Remove(TKey1 key1, TKey2 key2)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    _compositeKeyCollection.Remove(key1, key2);
                    scope.Complete();
                }
            }
            catch(Exception ex)
            {
            }
        }

        public int Count
        {
            get
            {
                return _compositeKeyCollection.Count;
            }
        }

        public virtual void Clear()
        {
            _compositeKeyCollection.Clear();
        }

        public virtual TData this[TKey1 key1, TKey2 key2]
        {
            get
            {
                if (key1 == null && key2 == null)
                    throw new ArgumentNullException("key1 and key2", "Key should not be nullable");

                return _compositeKeyCollection[key1, key2];
            }
            set
            {
                CheckKeyParameters(key1, key2);
                var key = new CompositeKey<TKey1, TKey2>(key1, key2);
                SetValue(key, value);
            }
        }
        public virtual KeyValuePair<CompositeKey<TKey1, TKey2>, TData> this[int index]
        {
            get
            {
                return _compositeKeyCollection[index];
            }
        }

        protected void CheckKeyParameters(TKey1 key1, TKey2 key2)
        {
            if (key1 == null)
                throw new ArgumentNullException("key1");

            if (key2 == null)
                throw new ArgumentNullException("key2");
        }

        private void SetValue(CompositeKey<TKey1, TKey2> key, TData data)
        {
            _compositeKeyCollection.Add(key.Id, key.Name, data);
        }
    }
}

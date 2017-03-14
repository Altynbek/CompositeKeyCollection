using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyCollectionTest.Classes
{
    public class CompositeKeyCollection<TKey1, Tkey2, TData>
    {
        #region Fields
        protected Dictionary<CompositeKey<TKey1, Tkey2>, TData> _dictionary = new Dictionary<CompositeKey<TKey1, Tkey2>, TData>();
        protected Dictionary<TKey1, Dictionary<Tkey2, TData>> _idDictionary = new Dictionary<TKey1, Dictionary<Tkey2, TData>>();
        protected Dictionary<Tkey2, Dictionary<TKey1, TData>> _namedDictionary = new Dictionary<Tkey2, Dictionary<TKey1, TData>>();
        #endregion


        #region public methods
        public virtual void Add(TKey1 key1, Tkey2 key2, TData data)
        {
            CheckKeyParameters(key1, key2);

            var key = new CompositeKey<TKey1, Tkey2>(key1, key2);
            _dictionary.Add(key, data);

            // add to id collection
            if (_idDictionary.ContainsKey(key1))
                _idDictionary[key1].Add(key2, data);
            else
                _idDictionary.Add(key1, new Dictionary<Tkey2, TData>() { { key2, data } });


            // add to named collection
            if (_namedDictionary.ContainsKey(key2))
                _namedDictionary[key2].Add(key1, data);
            else
                _namedDictionary[key2] = new Dictionary<TKey1, TData>() { { key1, data } };
        }


        public virtual void Remove(TKey1 key1, Tkey2 key2)
        {
            CheckKeyParameters(key1, key2);
            var key = new CompositeKey<TKey1, Tkey2>(key1, key2);

            if (_dictionary.ContainsKey(key))
                _dictionary.Remove(key);

            // remove from _idDictionary
            if (_idDictionary.ContainsKey(key1))
            {
                var dictionaryWithNames = _idDictionary[key1];
                if (dictionaryWithNames.ContainsKey(key2))
                    dictionaryWithNames.Remove(key2);

                if (dictionaryWithNames.Count == 0)
                    _idDictionary.Remove(key1);
            }

            // remove from _namedDictionary
            if (_namedDictionary.ContainsKey(key2))
            {
                var dictionaryWithId = _namedDictionary[key2];
                if (dictionaryWithId.ContainsKey(key1))
                    dictionaryWithId.Remove(key1);

                if (dictionaryWithId.Count == 0)
                    _namedDictionary.Remove(key2);
            }
        }

        public virtual bool ContainsKey(TKey1 key1, Tkey2 key2)
        {
            CheckKeyParameters(key1, key2);
            var key = new CompositeKey<TKey1, Tkey2>(key1, key2);
            return _dictionary.ContainsKey(key);
        }

        public virtual bool ContainsValue(TData data)
        {
            return _dictionary.ContainsValue(data);
        }

        public virtual void Clear()
        {
            _dictionary.Clear();
            _idDictionary.Clear();
            _namedDictionary.Clear();
        }

        public virtual TData this[TKey1 key1, Tkey2 key2]
        {
            get
            {
                CheckKeyParameters(key1, key2);
                var key = new CompositeKey<TKey1, Tkey2>(key1, key2);
                return _dictionary[key];
            }
            set
            {
                CheckKeyParameters(key1, key2);
                var key = new CompositeKey<TKey1, Tkey2>(key1, key2);
                SetValue(key, value);
            }
        }
        public virtual KeyValuePair<CompositeKey<TKey1, Tkey2>, TData> this[int index]
        {
            get
            {
                return _dictionary.ElementAt(index);
            }
        }

        public virtual Dictionary<CompositeKey<TKey1, Tkey2>, TData> Find(TKey1 key1, Tkey2 key2)
        {
            var result = new Dictionary<CompositeKey<TKey1, Tkey2>, TData>();

            if (key1 != null && key2 != null)                // получаем элемент из исходной коллекции по композитному ключу
            {
                var key = new CompositeKey<TKey1, Tkey2>(key1, key2);
                result.Add(key, _dictionary[key]);
            }
            else if (key1 != null)
            {
                var items = _idDictionary[key1];            // получаем по дате словарь из пар <name, value>
                for (int idx = 0; idx < items.Count; idx++)
                {
                    var currentPair = items.ElementAt(idx);
                    result.Add(new CompositeKey<TKey1, Tkey2>(key1, currentPair.Key), currentPair.Value);
                }
            }
            else if (key2 != null)                           // получаем по имени словарь из пар <id, value>
            {
                var items = _namedDictionary[key2];
                for (int idx = 0; idx < items.Count; idx++)
                {
                    var currentPair = items.ElementAt(idx);
                    result.Add(new CompositeKey<TKey1, Tkey2>(currentPair.Key, key2), currentPair.Value);
                }
            }

            return result;
        }


        #endregion


        #region properties
        public virtual Dictionary<CompositeKey<TKey1, Tkey2>, TData>.KeyCollection Keys
        {
            get
            {
                var lst = _dictionary.Keys;
                return lst;
            }
        }

        public virtual Dictionary<CompositeKey<TKey1, Tkey2>, TData>.ValueCollection Values
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


        #region private methods
        private void SetValue(CompositeKey<TKey1, Tkey2> key, TData data)
        {
            _dictionary.Add(key, data);
        }

        protected void CheckKeyParameters(TKey1 key1, Tkey2 key2)
        {
            if (key1 == null)
                throw new ArgumentNullException("key1");

            if (key2 == null)
                throw new ArgumentNullException("key2");
        }

        #endregion

    }
}

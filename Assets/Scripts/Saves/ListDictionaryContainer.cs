using System.Collections.Generic;
using System;

namespace SaveManager
{
    public interface IListDictionaryIdentifiable
    {
        public string ID { get; set; }
    }
    public class ListDictionaryIdentifiableBase: IListDictionaryIdentifiable
    {
        public string ID { get => Id; set => Id = value; }
        public string Id;
    }
    [Serializable]
    public class ListDictionaryContainer<T> where T : IListDictionaryIdentifiable
    {
        public List<T> Values = new List<T>();
        private Dictionary<string, T> _dictionaryCache;
        private Dictionary<string, T> EnsureCache()
        {
            if (_dictionaryCache == null)
            {
                _dictionaryCache = new Dictionary<string, T>();
                foreach (var kvp in Values)
                {
                    _dictionaryCache.Add(kvp.ID, kvp);
                }
            }
            return _dictionaryCache;
        }
        public bool TryGetValue(string key, out T result)
        {
            return EnsureCache().TryGetValue(key, out result);
        }
        public void SetValue(string key, in T result)
        {
            EnsureCache();
            result.ID = key;
            if (_dictionaryCache.ContainsKey(key))
            {
                Values.RemoveAll(x => x.ID == key);
                _dictionaryCache[result.ID] = result;
            }
            else
            {
                _dictionaryCache.Add(key, result);
            }
            Values.Add(result);
        }
        public T this[string key]
        {
            get => EnsureCache()[key]; set => SetValue(key, value);
        }
    }
}
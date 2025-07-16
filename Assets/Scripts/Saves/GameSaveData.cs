using System.Collections.Generic;
using static SaveManager.GameSaveData.LootboxData;
using Registries;
using System;

namespace SaveManager
{
    [System.Serializable]
    public class GameSaveData
    {
        [System.Serializable]
        public class SettingsData
        {
            public float MusicVolume = 1.0f;
            public float SFXVolume = 1.0f;
            public float CameraSensivity = 1.0f;
            public string CurrentLocaleName = "";
        }
        [System.Serializable]
        public class LootboxData : ListDictionaryContainer<LootboxData.Loot>
        {
            [System.Serializable]
            public class Loot: ListDictionaryIdentifiableBase
            {
                public int Count;
                public Loot(int count = 0)
                {
                    Count = count;
                }
            }
            public void Add(string id, int count = 1)
            {
                Loot found = Values.Find(x => x.ID == id);
                if (found != null) found.Count += count;
                else Values.Add(new Loot() { ID = id, Count = count });
            }
        }
        [System.Serializable]
        public class LevelStatsData : ListDictionaryContainer<LevelStatsData.LevelCompletion>
        {
            [System.Serializable]
            public class LevelCompletion : ListDictionaryIdentifiableBase
            {
                public float BestTime;
                public int BestScore;
                public LevelCompletion() { }
                public LevelCompletion(float bestTime, int bestScore)
                {
                    BestTime = bestTime;
                    BestScore = bestScore;
                }
            }
            public string LastPlayedLevelID;
        }
        public SettingsData Settings = new SettingsData();
        public LootboxData Loot = new LootboxData();
        public LevelStatsData LevelStats = new LevelStatsData();
    }
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
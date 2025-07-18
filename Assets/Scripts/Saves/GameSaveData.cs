using static SaveManager.GameSaveData.LootboxData;
using Registries;

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
            public bool ShowTutorial = true;
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

}
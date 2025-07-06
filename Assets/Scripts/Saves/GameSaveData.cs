using System.Collections.Generic;

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
        public class LootboxData
        {
            [System.Serializable]
            public class Loot
            {
                public string Identifier;
                public int Count;
            }
            public List<Loot> LootList = new List<Loot>();
            public void Add(string id, int count = 1)
            {
                Loot found = LootList.Find(x => x.Identifier == id);
                if (found != null) found.Count += count;
                else LootList.Add(new Loot() { Identifier = id, Count = count });
            }
        }
        [System.Serializable]
        public class LevelStatsData
        {
            [System.Serializable]
            public class LevelCompletion
            {
                public string Identifier;
                public float BestTime;
                public int BestScore;
            }
            public List<LevelCompletion> CompletedLevels = new List<LevelCompletion>();
            public void Update(string id, float bestTime, int bestScore)
            {
                LevelCompletion found = CompletedLevels.Find(x => x.Identifier == id);
                if (found != null)
                {
                    if (bestScore > found.BestScore) found.BestScore = bestScore;
                    if (bestTime < found.BestTime) found.BestTime = bestTime;
                }
                else CompletedLevels.Add(new LevelCompletion()
                { Identifier = id, BestScore = bestScore, BestTime = bestTime}
                );
            }
        }
        public SettingsData Settings = new SettingsData();
        public LootboxData Loot = new LootboxData();
        public LevelStatsData LevelStats = new LevelStatsData();
    }
}
using System.Collections.Generic;
using System.Threading;
using SaveManager;
using UnityEngine;
using VContainer.Unity;
namespace Gameplay.Loot
{
    public struct LootAndCount {
        public LootScriptableObject Loot;
        public int Count;
    }
    public interface IGameLootManager
    {
        public IRandom GetRandom();
        public void AddLoot(string key, int count = 1);
        public IEnumerable<LootAndCount> GetMyLoot();
        public IEnumerable<LootAndCount> GetAllPossibleLoot();
    }
    public class GameLootManager: IGameLootManager, IStartable
    {
        ILootGameSaveManager saveManager;
        LootRegistryScriptableObject lootRegistry;
        GameSaveData.LootboxData lootboxData;

        public GameLootManager(ILootGameSaveManager saveManager, LootRegistryScriptableObject registry)
        {
            this.saveManager = saveManager;
            lootboxData = new GameSaveData.LootboxData();
        }
        public void AddLoot(string key, int count = 1)
        {
            lootboxData.Add(key, count);
            saveManager.SetValue(lootboxData);
        }

        public IEnumerable<LootAndCount> GetAllPossibleLoot()
        {
            foreach (var kvpair in lootRegistry)
            {
                var res = new LootAndCount() { Count = 0, Loot = kvpair.Value };
                if (lootboxData.TryGetValue(kvpair.Key, out var result))
                {
                    res.Count = result.Count;
                }
                yield return res;
            }
        }

        public IEnumerable<LootAndCount> GetMyLoot()
        {
            foreach (var dat in lootboxData.Values)
            {
                var lt = lootRegistry.Get(dat.ID);
                if (lt != null)
                {
                    yield return new LootAndCount() { Count = dat.Count, Loot = lt };
                }
            }
        }

        public IRandom GetRandom()
        {
            return new BasicRandom();
        }

        public void Start()
        {
            lootboxData = this.saveManager.GetValue();
        }
    }
}

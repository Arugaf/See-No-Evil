using Leaderboard;

namespace SaveManager
{
    public interface ILootGameSaveManager : ISaveManager<GameSaveData.LootboxData>
    {
    }
    public class LootGameSaveManager : LeaderboardSaveManager<GameSaveData.LootboxData>, ILootGameSaveManager
    {
        public LootGameSaveManager(IGameSaveManager gameSaveManager, IGlobalLeaderboardScoreSaver saver) : base(gameSaveManager, saver)
        {
        }

        protected override GameSaveData.LootboxData Get(GameSaveData data) => data.Loot;

        protected override GameSaveData SetData(GameSaveData data, GameSaveData.LootboxData value) { data.Loot = value; return data; }
    }
}
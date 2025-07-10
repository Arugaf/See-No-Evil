namespace SaveManager
{
    public interface ILootGameSaveManager : ISaveManager<GameSaveData.LootboxData>
    {
    }
    public class LootGameSaveManager : ChildSaveManager<GameSaveData.LootboxData>, ILootGameSaveManager
    {
        public LootGameSaveManager(IGameSaveManager gameSaveManager) : base(gameSaveManager)
        {
        }

        protected override GameSaveData.LootboxData Get(GameSaveData data) => data.Loot;

        protected override GameSaveData Set(GameSaveData data, GameSaveData.LootboxData value) { data.Loot = value; return data; }
    }
}
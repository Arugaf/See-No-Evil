namespace SaveManager
{
    public interface ILevelStatsSaveManager : ISaveManager<GameSaveData.LevelStatsData>
    {
    }
    public class LevelStatsSaveManager : ChildSaveManager<GameSaveData.LevelStatsData>, ILevelStatsSaveManager
    {
        public LevelStatsSaveManager(IGameSaveManager gameSaveManager) : base(gameSaveManager)
        {
        }

        protected override GameSaveData.LevelStatsData Get(GameSaveData data) => data.LevelStats;

        protected override GameSaveData Set(GameSaveData data, GameSaveData.LevelStatsData value) { data.LevelStats = value; return data; }
    }
}
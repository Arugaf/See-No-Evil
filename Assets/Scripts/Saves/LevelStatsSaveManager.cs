using Leaderboard;

namespace SaveManager
{
    public interface ILevelStatsSaveManager : ISaveManager<GameSaveData.LevelStatsData>
    {
    }
    public class LevelStatsSaveManager : LeaderboardSaveManager<GameSaveData.LevelStatsData>, ILevelStatsSaveManager
    {
        public LevelStatsSaveManager(IGameSaveManager gameSaveManager, IGlobalLeaderboardScoreSaver saver) : base(gameSaveManager, saver)
        {
        }

        protected override GameSaveData.LevelStatsData Get(GameSaveData data) => data.LevelStats;

        protected override GameSaveData SetData(GameSaveData data, GameSaveData.LevelStatsData value) { data.LevelStats = value; return data; }
    }
}
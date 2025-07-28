using Cysharp.Threading.Tasks;
using Leaderboard;
namespace SaveManager
{
    /// <summary>
    /// The parent is always IGameSaveManager
    /// </summary>
    public abstract class ChildSaveManager<T> : ISaveManager<T>
    {
        protected IGameSaveManager gameSaveManager;
        public ChildSaveManager(IGameSaveManager gameSaveManager)
        {
            this.gameSaveManager = gameSaveManager;
        }
        protected abstract T Get(GameSaveData data);
        protected abstract GameSaveData Set(GameSaveData data, T value);
        public T GetValue() => Get(gameSaveManager.GetValue());
        public void SetValue(T value) => gameSaveManager.SetValue(Set(gameSaveManager.GetValue(), value));
        public UniTask Save() => gameSaveManager.Save();

        public UniTask Load() => gameSaveManager.Load();
    }
    public abstract class LeaderboardSaveManager<T> : ChildSaveManager<T>
    {
        private IGlobalLeaderboardScoreSaver scoreSaver;
        protected LeaderboardSaveManager(IGameSaveManager gameSaveManager, IGlobalLeaderboardScoreSaver scoreSaver) : base(gameSaveManager)
        {
            this.scoreSaver = scoreSaver;
        }
        protected sealed override GameSaveData Set(GameSaveData data, T value)
        {
            var val = SetData(data, value);
            val = scoreSaver.UpdateGlobalScore(data);
            return val;
        }
        protected abstract GameSaveData SetData(GameSaveData data, T value);
    }
}
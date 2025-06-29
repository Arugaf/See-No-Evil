using Cysharp.Threading.Tasks;
namespace SaveManager
{
    /// <summary>
    /// The parent is always IGameSaveManager
    /// </summary>
    public abstract class ChildSaveManager<T>: ISaveManager<T>
    {
        protected IGameSaveManager gameSaveManager;
        public ChildSaveManager(IGameSaveManager gameSaveManager)
        {
            this.gameSaveManager = gameSaveManager;
        }
        protected abstract T Get(GameSaveData data);
        protected abstract void Set(GameSaveData data, T value);
        public T GetValue() => Get(gameSaveManager.GetValue());
        public void SetValue(T value) => Set(gameSaveManager.GetValue(), value);
        public UniTask Save() => gameSaveManager.Save();

        public UniTask Load() => gameSaveManager.Load();
    }
}
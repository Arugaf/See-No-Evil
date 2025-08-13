using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System.Threading.Tasks;
using VContainer;
namespace SaveManager
{
    public interface ISaveManager<T>
    {
        public T GetValue();
        public void SetValue(T value);
        public UniTask Save();
        public UniTask Load();
    }

    /// <summary>
    /// The ROOT.
    /// </summary>
    public interface IGameSaveManager: ISaveManager<GameSaveData>
    {
    }
    public static class SaveManagerInstaller
    {
        public static void UseHierachyInstallment(IContainerBuilder builder)
        {
            builder.Register<ISettingSaveManager, SettingsSaveManager>(Lifetime.Singleton);
            builder.Register<ILootGameSaveManager, LootGameSaveManager>(Lifetime.Singleton);
            builder.Register<ILevelStatsSaveManager, LevelStatsSaveManager>(Lifetime.Singleton);
        }
    }
}
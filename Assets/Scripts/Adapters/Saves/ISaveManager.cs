using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine;
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
    public interface ISettingSaveManager: ISaveManager<GameSaveData.SettingsData>
    {
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
        }
    }
}
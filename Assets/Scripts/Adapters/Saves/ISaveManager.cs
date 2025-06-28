using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine;
using VContainer;
namespace SaveManager
{
    [System.Serializable]
    public class GameSaveData
    {
        [System.Serializable]
        public class SettingsData
        {
            public float MusicVolume;
            public float SFXVolume;
            public float CameraSensivity;
        }
        public SettingsData Settings;
    }
    public interface ISaveManager<T>
    {
        public UniTask<T> Load(bool reload = false);
        public UniTask Save(T data);
    }
    public abstract class HierarchySaveManager<T>: ISaveManager<T>
    {
        protected IGameSaveManager gameSaveManager;
        public HierarchySaveManager(IGameSaveManager gameSaveManager)
        {
            this.gameSaveManager = gameSaveManager;
        }
        protected abstract T Get(GameSaveData data);
        protected abstract void Set(GameSaveData data, T value);

        public async UniTask Save(T data)
        {
            GameSaveData d = await gameSaveManager.Load();
            Set(d, data);
            await gameSaveManager.Save(d);
        }

        public async UniTask<T> Load(bool r)
        {
            return Get(await gameSaveManager.Load(r));
        }
    }
    public class SettingsSaveManager : HierarchySaveManager<GameSaveData.SettingsData>, ISettingSaveManager
    {
        public SettingsSaveManager(IGameSaveManager gameSaveManager) : base(gameSaveManager)
        {
        }

        protected override GameSaveData.SettingsData Get(GameSaveData data) => data.Settings;

        protected override void Set(GameSaveData data, GameSaveData.SettingsData value) => data.Settings = value;
    }
    public interface ISettingSaveManager: ISaveManager<GameSaveData.SettingsData>
    {
    }
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
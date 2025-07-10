using System.Collections;

namespace SaveManager
{

    public interface ISettingSaveManager : ISaveManager<GameSaveData.SettingsData>
    {
    }

    public class SettingsSaveManager : ChildSaveManager<GameSaveData.SettingsData>, ISettingSaveManager
    {
        public SettingsSaveManager(IGameSaveManager gameSaveManager) : base(gameSaveManager)
        {
        }

        protected override GameSaveData.SettingsData Get(GameSaveData data) => data.Settings;

        protected override GameSaveData Set(GameSaveData data, GameSaveData.SettingsData value) { data.Settings = value; return data; }
    }
}
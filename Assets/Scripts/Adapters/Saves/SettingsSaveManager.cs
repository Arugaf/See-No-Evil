namespace SaveManager
{
    public class SettingsSaveManager : ChildSaveManager<GameSaveData.SettingsData>, ISettingSaveManager
    {
        public SettingsSaveManager(IGameSaveManager gameSaveManager) : base(gameSaveManager)
        {
        }

        protected override GameSaveData.SettingsData Get(GameSaveData data) => data.Settings;

        protected override void Set(GameSaveData data, GameSaveData.SettingsData value) => data.Settings = value;
    }
}
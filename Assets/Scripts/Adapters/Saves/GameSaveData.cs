namespace SaveManager
{
    [System.Serializable]
    public class GameSaveData
    {
        [System.Serializable]
        public class SettingsData
        {
            public float MusicVolume = 1.0f;
            public float SFXVolume = 1.0f;
            public float CameraSensivity = 1.0f;
        }
        public SettingsData Settings = new SettingsData();
    }
}
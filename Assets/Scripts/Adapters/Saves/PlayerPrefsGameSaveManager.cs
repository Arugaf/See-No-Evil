using Cysharp.Threading.Tasks;
using UnityEngine;
namespace SaveManager
{
    public class PlayerPrefsGameSaveManager : IGameSaveManager
    {
        private GameSaveData SaveData = new GameSaveData();
        private bool loaded = false;
        public GameSaveData GetValue()
        {
            LoadIfNeeded();
            return SaveData;
        }
        private void LoadIfNeeded()
        {
            if (!loaded)
            {
                string q = PlayerPrefs.GetString(nameof(SaveData));
                SaveData = JsonUtility.FromJson(q, typeof(GameSaveData)) as GameSaveData;
                SaveData ??= new GameSaveData();
                loaded = true;
            }
        }
        public UniTask Load()
        {
            LoadIfNeeded();
            return UniTask.CompletedTask;
        }

        public UniTask Save()
        {
            DoSave();
            PlayerPrefs.Save();
            return UniTask.CompletedTask;
        }
        private void DoSave()
        {
            string json = JsonUtility.ToJson(SaveData);
            PlayerPrefs.SetString(nameof(SaveData), json);
        }
        public void SetValue(GameSaveData value)
        {
            SaveData = value;
            DoSave();
        }
    }
}
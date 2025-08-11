using Cysharp.Threading.Tasks;
using SaveManager;
using UnityEngine;
using YG;
namespace YG
{
    public partial class SavesYG
    {
        public GameSaveData SaveData;
    }
}
namespace SaveManager
{

    public class PluginYGGameSaveManager : IGameSaveManager
    {
        public GameSaveData GetValue() => YG2.saves.SaveData ?? new GameSaveData();
        private UniTask? cycleSave;
        private float savePeriod = 1.0f;
        private bool dirty = false;
        public UniTask Load()
        {
            return UniTask.CompletedTask;
        }

        public UniTask Save()
        {
            YG2.SaveProgress();
            return UniTask.CompletedTask;
        }

        public void SetValue(GameSaveData value)
        {
            YG2.saves.SaveData = value;
            SetDirty();
        }
        private void SetDirty()
        {
            dirty = true;
            if (cycleSave == null)
            {
                cycleSave = Cycles();
                cycleSave.Value.Forget();
                Debug.Log("Saving progress delay...");
            }
        }
        private async UniTask Cycles()
        {
            do
            {
                dirty = false;
                await UniTask.WaitForSeconds(savePeriod, true);
            }
            while (dirty);
            YG2.SaveProgress();
            cycleSave = null;
        }
    }
}

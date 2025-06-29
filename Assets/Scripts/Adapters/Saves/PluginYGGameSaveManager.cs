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

        public UniTask Load()
        {
            return UniTask.CompletedTask;
        }

        public UniTask Save()
        {
            YG2.SaveProgress();
            return UniTask.CompletedTask;
        }

        public void SetValue(GameSaveData value) => YG2.saves.SaveData = value;
    }
}

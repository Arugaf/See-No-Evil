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
        public UniTask<GameSaveData> Load(bool reload)
        {
            return UniTask.FromResult(YG2.saves.SaveData);
        }

        public UniTask Save(GameSaveData data)
        {
            YG2.saves.SaveData = data;
            YG2.SaveProgress();
            return UniTask.CompletedTask;
        }
    }
}

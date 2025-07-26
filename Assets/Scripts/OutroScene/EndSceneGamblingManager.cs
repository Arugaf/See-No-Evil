using Cysharp.Threading.Tasks;
using Gameplay;
using Gameplay.Loot;
using Monetization;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using VContainer;
namespace Features.OutroScene
{
    public abstract class AbstractEndSceneGamblingController: MonoBehaviour
    {
        public abstract UniTask ShowObject(GambleBoxLootObject obj);
        public abstract UniTask<LootAndCount> DoPick(IRandom rnd, GambleBoxLootObject loot);
    }
    public class EndSceneGamblingManager : EndSceneManagerBehaviour
    {
        [SerializeField] private AbstractEndSceneGamblingController controller;
        private IGameLootManager lootManager;
        private GameplayResultStorage res;

        [Inject]
        private void Construct(IGameLootManager manager, GameplayResultStorage res)
        {
            lootManager = manager;
            this.res = res;
        }
        bool DoGivePrize { get => res.AquiredPrize && res.LastGameState == GameplayResultStorage.Result.Victory; }
        public async override UniTask Init()
        {
            if (DoGivePrize)
            {
                await controller.ShowObject(res.gameLevelInfo.RandomLootObject);
            }
        }
        public async override UniTask DoProcess()
        {
            if (DoGivePrize)
            {
                var result = await controller.DoPick(lootManager.GetRandom(), res.gameLevelInfo.RandomLootObject);
                lootManager.AddLoot(result.Loot.ID, result.Count);
            }
        }
    }
}
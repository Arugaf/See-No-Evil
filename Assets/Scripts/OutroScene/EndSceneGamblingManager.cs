using Cysharp.Threading.Tasks;
using Gameplay;
using Gameplay.Loot;
using Monetization;
using UnityEngine;
using VContainer;
namespace Features.OutroScene
{
    public abstract class EndSceneGamblingController: MonoBehaviour
    {
        public abstract UniTask<LootAndCount> DoPick(IRandom rnd, GambleBoxLootObject loot);
    }
    public class EndSceneGamblingManager : EndSceneManagerBehaviour
    {
        [SerializeField] private EndSceneGamblingController controller;
        private IGameLootManager lootManager;
        private GameplayResultStorage res;

        [Inject]
        private void Construct(IGameLootManager manager, GameplayResultStorage res)
        {
            lootManager = manager;
            this.res = res;
        }
        public async override UniTask DoProcess()
        {
            if (res.AquiredPrize)
            {
                var result = await controller.DoPick(lootManager.GetRandom(), res.gameLevelInfo.RandomLootObject);
                lootManager.AddLoot(result.Loot.ID, result.Count);
            }
        }
    }
}
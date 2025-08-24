using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Features.OutroScene;
using Gameplay.Loot;
using Monetization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VContainer;
namespace Features.OutroScene
{
    public class GamblingController : AbstractEndSceneGamblingController
    {
        [SerializeField] private GamblingControllerSegmentSetupper setupper;
        [SerializeField] private Button proceedButton;
        [SerializeField] private Button adRetryButton;
        [SerializeField] private Button confirmButton;
                [SerializeField] private int retryCount = 1;
        private IAdManager adManager;
        [Inject]
        private void Construct(IAdManager adManager)
        {
            this.adManager = adManager;
        }
        private UniTask<int> WaitForButtonToClick(params Button[] btn)
        {
            UniTaskCompletionSource<int> src = new UniTaskCompletionSource<int>();
            Debug.Log("BUTTONS");
            for (int i = 0; i < btn.Length; i++)
            {
                int copyidx = i;
                UnityAction listener = () =>
                {
                    src.TrySetResult(copyidx);
                    foreach (Button b in btn) b.onClick.RemoveAllListeners();
                };
                btn[i].onClick.AddListener(listener);
            }
            return src.Task;
        }
        private async UniTask<bool> TryConfirmLoot()
        {
            bool unconfirmed = true;
            bool ret = true;
            do
            {
                int idx = await WaitForButtonToClick(adRetryButton, confirmButton);
                Debug.Log("BUTTONS DONE");
                if (idx == 0)
                {
                    var result = await adManager.ShowRewardedAdverticement();
                    Debug.Log($"REV SUCCESS: {result.IsSuccess}");
                    if (result.IsSuccess)
                    {
                        ret = false;
                        unconfirmed = false;
                    }
                }
                else unconfirmed = false;
                Debug.Log("ITER");
                await UniTask.WaitForEndOfFrame();
            } while (unconfirmed);
            return ret;
        }
        public override async UniTask<LootAndCount> DoPick(IRandom rnd, GambleBoxLootObject loot)
        {
            // warning: current logic supports pickTimes >= 1. Zero would be counted as 1.
            int pickTimes = retryCount;
            bool retry = true;
            LootAndCount result;
            await setupper.DoSetActive(true);
            adRetryButton.gameObject.SetActive(adManager.RewardedAdsAvailable);
            do
            {
                setupper.StartSegment();
                await WaitForButtonToClick(proceedButton);
                var pick = loot.Pick(rnd);
                result = new LootAndCount() { Loot = pick, Count = 1 };
                await setupper.EndSegment(pick);
                retry = !await TryConfirmLoot();
                if (retry)
                {
                    pickTimes--;
                    if (pickTimes <= 0) adRetryButton.gameObject.SetActive(false);
                }
            } while (retry);
            await setupper.DoSetActive(false);
            return result;
        }

        public override UniTask ShowObject(GambleBoxLootObject obj)
        {
            return setupper.InitObject(obj);
        }
    }
}
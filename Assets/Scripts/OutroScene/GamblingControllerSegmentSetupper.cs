using Cysharp.Threading.Tasks;
using Features.IntroScene;
using Gameplay.Loot;
using UnityEngine;
namespace Features.OutroScene
{
    public class GamblingControllerSegmentSetupper : MonoBehaviour
    {
        [SerializeField] private Animator mainUIAnimator;
        [SerializeField] private Animator basicUIAnimator;
        [SerializeField] private float transitionDelay = 1.5f;
        [SerializeField] private LocalizedTextController textController;
        [SerializeField] private CameraLookIntroSceneSubcomponent firstCam;
        [SerializeField] private CameraLookIntroSceneSubcomponent secondCam;
        [SerializeField] private Transform gambleBoxTransform;
        [SerializeField] private GamblingItemView gamblingItemView;
        private IGambleBoxView gambleBoxView;

        public UniTask InitObject(GambleBoxLootObject lootObject)
        {
            gambleBoxView = GameObject.Instantiate(lootObject.ViewPrefab, gambleBoxTransform).GetComponent<IGambleBoxView>();
            firstCam.enabled = true;
            secondCam.enabled = true;
            return UniTask.CompletedTask;
        }
        public void StartSegment()
        {
            basicUIAnimator.SetBool("EndSegment", false);
            gambleBoxView.SetOpen(false);
            firstCam.gameObject.SetActive(true);

            secondCam.gameObject.SetActive(false);
            gamblingItemView.enabled = false;
        }
        public async UniTask EndSegment(LootScriptableObject obj)
        {
            gambleBoxView.SetOpen(true);
            await gamblingItemView.ToShow(obj);
            basicUIAnimator.SetBool("EndSegment", true);
            firstCam.gameObject.SetActive(false);
            secondCam.gameObject.SetActive(true);
            await textController.SetText(obj.Name);
            gamblingItemView.enabled = true;
        }
        public async UniTask DoSetActive(bool active)
        {
            if (active)
            {
                mainUIAnimator.SetBool("Hide", true);
                basicUIAnimator.gameObject.SetActive(true);
                firstCam.gameObject.SetActive(true);
                secondCam.gameObject.SetActive(false);
                await UniTask.WaitForSeconds(transitionDelay);
            }
            else
            {
                firstCam.gameObject.SetActive(false);
                secondCam.gameObject.SetActive(false);
                mainUIAnimator.SetBool("Hide", false);
                basicUIAnimator.SetBool("Hide", true);
                await UniTask.WaitForSeconds(transitionDelay);
                basicUIAnimator.gameObject.SetActive(false);
            }
        }
    }
}
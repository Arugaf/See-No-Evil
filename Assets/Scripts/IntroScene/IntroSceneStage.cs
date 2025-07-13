using Cysharp.Threading.Tasks;
using SaveManager;
using Unity.Cinemachine;
using UnityEngine;
namespace Features.IntroScene
{
    public abstract class AbstractIntroSceneStage : MonoBehaviour, IListDictionaryIdentifiable
    {
        [field:SerializeField] public string ID { get; set; }
 
        public abstract UniTask SetActivation(bool active);
    }
    public class IntroSceneStageSubcomponent : MonoBehaviour
    {
        public virtual void IntentionForActivation(bool state){}
        public virtual void SetActivation(bool state) { enabled = state; }
    }
    public class IntroSceneStage : AbstractIntroSceneStage
    {
        private const string KEY_ANIM_HIDDEN = "Hidden";
        [SerializeField] private CinemachineCamera cinemachineCamera;
        [SerializeField] private Animator transitorAnim;
        [SerializeField] private GameObject mainCanvasObject;
        [SerializeField, Min(0)] private float transitionEnableDuration = 1.0f;
        [SerializeField, Min(0)] private float transitionDisableDuration = 1.0f;
        [SerializeField] private IntroSceneStageSubcomponent[] Subcomponents;
        private bool isActive;
        private bool transition;
        public async UniTask Disabling()
        {
            transition = true;
            isActive = false;
            cinemachineCamera.enabled = false;
            transitorAnim.SetBool(KEY_ANIM_HIDDEN, true);
            await UniTask.WaitForSeconds(transitionDisableDuration);
            IntentionSetEnabledSubcomponents(false);
            mainCanvasObject.SetActive(false);
            SetEnabledSubcomponents(false);
            transition = false;
        }
        private void IntentionSetEnabledSubcomponents(bool state)
        {
             foreach (var sub in Subcomponents) sub.IntentionForActivation(state);
        }
        private void SetEnabledSubcomponents(bool state)
        {
            foreach (var sub in Subcomponents) sub.SetActivation(state);
        }
        public async UniTask Enabling()
        {
            transition = true;
            isActive = true;
            cinemachineCamera.enabled = true;
            mainCanvasObject.SetActive(true);
            transitorAnim.SetBool(KEY_ANIM_HIDDEN, false);
            IntentionSetEnabledSubcomponents(true);
            await UniTask.WaitForSeconds(transitionEnableDuration);
            SetEnabledSubcomponents(true); 
            transition = false;
        }

        public override UniTask SetActivation(bool active)
        {
            if (transition) return UniTask.CompletedTask;
            if (active) return Enabling();
            else return Disabling();
        }
    }
}
using Cysharp.Threading.Tasks;
using Features.IntroScene;
using Features.VFX;
using UnityEngine;
namespace Features.OutroScene
{
    public class EndSceneTransitionManager : EndSceneManagerBehaviour
    {
        [SerializeField] private Animator sceneAnimator;
        [SerializeField] private float startMenuAnimation = 1.5f;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private CameraLookIntroSceneSubcomponent subcomponent;
        [SerializeField] private IntroSceneDarknessRegulator regulator;
        [SerializeField] private float transitionTime;
        private SmoothDampArticulator farPlaneArticullator;
        public void Awake()
        {
            farPlaneArticullator = new SmoothDampArticulator(mainCamera.farClipPlane, 1.0f);
        }
        public override UniTask TransitionProcess(bool toGameplay)
        {
            sceneAnimator.SetBool("Hide", true);
            if (toGameplay)
            {
                regulator.SetDarknessFactor(1);
            }
            else
            {
                farPlaneArticullator.Target = mainCamera.nearClipPlane + 1.0f;
            }
            return UniTask.WaitForSeconds(transitionTime);
        }
        public override UniTask DoProcess()
        {
            subcomponent.IntentionForActivation(true);
            subcomponent.SetActivation(true);
            return UniTask.WaitForSeconds(startMenuAnimation);
        }
        public void Update()
        {
            if (Time.timeScale != 0)
                farPlaneArticullator.Update();
            mainCamera.farClipPlane = farPlaneArticullator.Current;
        }
    }
}
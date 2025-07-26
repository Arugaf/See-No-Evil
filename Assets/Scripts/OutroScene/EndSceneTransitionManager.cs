using Cysharp.Threading.Tasks;
using Features.IntroScene;
using Features.VFX;
using Unity.Cinemachine;
using UnityEngine;
namespace Features.OutroScene
{
    public class EndSceneTransitionManager : EndSceneManagerBehaviour
    {
        [SerializeField] private Animator sceneAnimator;
        [SerializeField] private float startMenuAnimation = 1.5f;
        [SerializeField] private float farPlaneSmoothTime = 0.7f;
        [SerializeField] private CinemachineCamera mainCamera;
        [SerializeField] private CameraLookIntroSceneSubcomponent subcomponent;
        [SerializeField] private IntroSceneDarknessRegulator regulator;
        [SerializeField] private float transitionTime;
        private SmoothDampArticulator farPlaneArticullator;
        public void Awake()
        {
            farPlaneArticullator = new SmoothDampArticulator(mainCamera.Lens.FarClipPlane, farPlaneSmoothTime);
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
                farPlaneArticullator.Target = mainCamera.Lens.NearClipPlane + 1.0f;
            }
            return UniTask.WaitForSeconds(transitionTime);
        }
        public override UniTask DoProcess()
        {
            regulator.SetDarknessFactor(0);
            subcomponent.IntentionForActivation(true);
            subcomponent.SetActivation(true);
            return UniTask.WaitForSeconds(startMenuAnimation);
        }
        public void Update()
        {
            if (Time.timeScale != 0)
                farPlaneArticullator.Update();
            mainCamera.Lens.FarClipPlane = farPlaneArticullator.Current;
        }
    }
}
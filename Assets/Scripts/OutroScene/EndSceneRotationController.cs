using Features.VFX;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.Events;
using YG;
using VContainer;
using Monetization;
using Cysharp.Threading.Tasks;
namespace Features.IntroScene
{
    public class EndSceneRotationController: MonoBehaviour
    {
        private InputActionAsset uiMainAsset;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private float xMaxAngle;
        [SerializeField] private float yMaxAngle;
        [SerializeField] private float smoothTime;
        [SerializeField] private float transitionTime;
        [SerializeField] private IntroSceneDarknessRegulator regulator;
        [SerializeField] private UnityEvent OnTransition;
        private bool transitioning = false;
        private SmoothDampArticulator xAngleArticulator;
        private SmoothDampArticulator yAngleArticulator;
        private SmoothDampArticulator farPlaneArticullator;

        private InputAction mouseActionMap;
        private IAdManager adManager;

        private Vector2 mouseScreenPoint;
        private Vector2 centerAlignedPoint;
        private void Start()
        {
            mouseActionMap = uiMainAsset.FindAction("Point");
            xAngleArticulator = new SmoothDampArticulator(0, smoothTime);
            yAngleArticulator = new SmoothDampArticulator(0, smoothTime);
            farPlaneArticullator = new SmoothDampArticulator(mainCamera.farClipPlane, smoothTime);
            regulator.SetDarknessFactor(0);
            adManager?.PreloadAdvertisement().Forget();
        }
        [Inject]
        private void Initialize(IAdManager manager, InputActionAsset asset)
        {
            uiMainAsset = asset;
            adManager = manager;
        }
        private void ReadInputs()
        {
            mouseScreenPoint = mouseActionMap.ReadValue<Vector2>();
            centerAlignedPoint = mouseScreenPoint;
            // Aling to center
            centerAlignedPoint.x /= Screen.width;
            centerAlignedPoint.y /= Screen.height;
            centerAlignedPoint -= new Vector2(0.5f, 0.5f);
            centerAlignedPoint *= 2;
        }
        private void UpdateCameraRotation()
        {
            xAngleArticulator.Target = centerAlignedPoint.x * xMaxAngle;
            yAngleArticulator.Target = centerAlignedPoint.y * yMaxAngle;

            mainCamera.transform.rotation = Quaternion.Euler(-yAngleArticulator.Current, xAngleArticulator.Current, 0);
            xAngleArticulator.Update();
            yAngleArticulator.Update();
        }
        private void Update()
        {
            ReadInputs();
            UpdateCameraRotation();
            if(Time.timeScale != 0)
                farPlaneArticullator.Update();
            mainCamera.farClipPlane = farPlaneArticullator.Current;
        }
        public void TransitionToGameplay()
        {
            if (transitioning) return;
            transitioning = true;
            StartCoroutine(DoTransition(true));

        }
        private IEnumerator DoTransition(bool toGameplay)
        {
            yield return adManager?.ShowAdvertisement().ToCoroutine();
            if (toGameplay)
            {
                regulator.SetDarknessFactor(1);
            }
            else
            {
                farPlaneArticullator.Target = mainCamera.nearClipPlane + 1.0f;
            }
            OnTransition?.Invoke();
            yield return new WaitForSeconds(transitionTime);
            if (!toGameplay)
            {
                GameStateManager.LoadIntroScene();
            }
            else
            {
                GameStateManager.LoadGameScene();
            }
        }
        private void OnDestroy()
        {
            adManager?.Dispose();
        }
        public void TransitionToMenu()
        {
            if (transitioning) return;
            transitioning = true;
            StartCoroutine(DoTransition(false));
        }

    }
}

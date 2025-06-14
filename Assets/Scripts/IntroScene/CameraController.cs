using Features.VFX;
using UnityEngine;
using Features.AudioManager;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.Events;
namespace Features.IntroScene
{
    public class CameraController: MonoBehaviour
    {
        [SerializeField] private InputActionAsset uiMainAsset;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private float xMaxAngle;
        [SerializeField] private float yMaxAngle;
        [SerializeField] private float smoothTime;
        [SerializeField] private AudioStepMaterial clickStepMaterial;
        [SerializeField] private IntroSceneDarknessRegulator darknessRegulator;
        [SerializeField] private float triggerRadius = 0.2f;
        [SerializeField] private UnityEvent OnEnter;
        [SerializeField] private float transitionDuration = 3.0f;
        [SerializeField] private float enableTransitionCountdown = 1.0f;
        
        private InputAction mouseActionMap;
        private InputAction mouseClickActionMap;
        private SmoothDampArticulator xAngleArticulator;
        private SmoothDampArticulator yAngleArticulator;
        private SmoothDampArticulator cameraFarPlane;
        private Vector2 mouseScreenPoint;
        private Vector2 centerAlignedPoint;
        private RaycastHit? lastHit;
        private bool toggle = false;
        private Coroutine transitionCoroutine;
        private bool interactable = true;

        public void SetInteractableState(bool state)
        {
            interactable = state;
        }

        public void Start()
        {
            mouseActionMap = uiMainAsset.FindAction("Point");
            mouseClickActionMap = uiMainAsset.FindAction("Click");
            mouseClickActionMap.performed += MouseClickActionMap_performed;
            destroyCancellationToken.Register(() => { mouseClickActionMap.performed -= MouseClickActionMap_performed; });
            xAngleArticulator = new SmoothDampArticulator(0, smoothTime);
            yAngleArticulator = new SmoothDampArticulator(0, smoothTime);
            cameraFarPlane = new SmoothDampArticulator(mainCamera.nearClipPlane + 1.0f, smoothTime);
            cameraFarPlane.Target = mainCamera.farClipPlane;
            mainCamera.farClipPlane = mainCamera.nearClipPlane + 1.0f;
            transitionCoroutine = null;
        }

        private void MouseClickActionMap_performed(InputAction.CallbackContext obj)
        {
            if (transitionCoroutine != null || !interactable) return;
            toggle = !toggle;
            if (obj.performed && toggle)
            {
                Raycast();
            }
            if(centerAlignedPoint.magnitude < triggerRadius && enableTransitionCountdown <= 0)
            {
                OnEnter?.Invoke();
                transitionCoroutine = StartCoroutine(ActionSequence());

            }
        }
        private IEnumerator ActionSequence()
        {
            darknessRegulator.SetDarknessFactor(1);
            yield return new WaitForSeconds(transitionDuration);
            GameStateManager.LoadGameScene();
        }
        private void Update()
        {
            xAngleArticulator.Update();
            yAngleArticulator.Update();
            cameraFarPlane.Update();
            mainCamera.farClipPlane = cameraFarPlane.Current;
            mouseScreenPoint = mouseActionMap.ReadValue<Vector2>();
            centerAlignedPoint = mouseScreenPoint;
            // Aling to center
            centerAlignedPoint.x /= Screen.width;
            centerAlignedPoint.y /= Screen.height;
            centerAlignedPoint -= new Vector2(0.5f, 0.5f);
            centerAlignedPoint *= 2;
            if(transitionCoroutine == null)
            {
                if (enableTransitionCountdown > 0) enableTransitionCountdown -= Time.deltaTime;
                else darknessRegulator.SetDarknessFactor(Mathf.Min(1, 1 + triggerRadius - centerAlignedPoint.magnitude));
            }
            xAngleArticulator.Target = centerAlignedPoint.x * xMaxAngle;
            yAngleArticulator.Target = centerAlignedPoint.y * yMaxAngle;

            mainCamera.transform.rotation = Quaternion.Euler(-yAngleArticulator.Current, xAngleArticulator.Current, 0);
            Ray ray = mainCamera.ScreenPointToRay(mouseScreenPoint);
            if (!Physics.Raycast(ray, out RaycastHit lst)) lastHit = null;
            else lastHit = lst;

        }
        void Raycast()
        {
            if(lastHit != null)
            {
                AudioManager.AudioManager.PlayAtomic(lastHit.Value.point, clickStepMaterial.Generate());
            }
            
        }
    }
}

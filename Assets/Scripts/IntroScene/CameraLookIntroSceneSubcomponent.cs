using Features.IntroScene;
using Features.VFX;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
namespace Features.IntroScene
{
    public class CameraLookIntroSceneSubcomponent : IntroSceneStageSubcomponent
    {
        private InputAction mouse;
        [SerializeField] private float smoothTime;
        private SmoothDampArticulator xAngleArticulator;
        private SmoothDampArticulator yAngleArticulator;
        [SerializeField] private float xMaxAngle;
        [SerializeField] private float yMaxAngle;
        private Quaternion originRotation;
        public override void IntentionForActivation(bool state)
        {
            if (state) enabled = true;
        }
        public override void SetActivation(bool state)
        {
            if (!state) enabled = false;
        }
        [Inject]
        private void Construct(InputActionAsset asset)
        {
            mouse = asset.FindAction("Point");
            xAngleArticulator = new SmoothDampArticulator(0, smoothTime);
            yAngleArticulator = new SmoothDampArticulator(0, smoothTime);
            originRotation = transform.localRotation;
            enabled = false;
        }
        // Update is called once per frame
        void Update()
        {
            Vector2 mouseScreenPoint = mouse.ReadValue<Vector2>();
            Vector2 centerAlignedPoint = mouseScreenPoint;
            // Aling to center
            centerAlignedPoint.x /= Screen.width;
            centerAlignedPoint.y /= Screen.height;
            centerAlignedPoint -= new Vector2(0.5f, 0.5f);
            centerAlignedPoint *= 2;
            xAngleArticulator.Target = centerAlignedPoint.x * xMaxAngle;
            yAngleArticulator.Target = centerAlignedPoint.y * yMaxAngle;
            xAngleArticulator.Update();
            yAngleArticulator.Update();
            transform.localRotation = originRotation * Quaternion.Euler(-yAngleArticulator.Current, xAngleArticulator.Current, 0);
        }
    }
}
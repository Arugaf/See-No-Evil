using Features.VFX;
using UnityEngine;
namespace Features.IntroScene {
    public class IntroSceneLightSubcomponent : IntroSceneStageSubcomponent
    {
        [SerializeField] private Light lightComponent;
        [SerializeField] private float smoothTime;
        private SmoothDampArticulatorToMultiplier articulator;
        private void Awake()
        {
            articulator = new SmoothDampArticulatorToMultiplier(0, lightComponent.intensity, smoothTime, float.MaxValue);
            lightComponent.intensity = 0;
            enabled = false;
        }
        public override void SetActivation(bool state)
        {
            
        }
        public override void IntentionForActivation(bool act)
        {
            Activate(act);
        }
        private void Activate(bool state)
        {
            articulator.TargetRatio = state ? 1 : 0;
            enabled = true;
        }
        void Update()
        {
            articulator.Update();
            if (Mathf.Abs(articulator.Current - lightComponent.intensity) < float.Epsilon) enabled = false;
            lightComponent.intensity = articulator.Current;
        }
    }
}
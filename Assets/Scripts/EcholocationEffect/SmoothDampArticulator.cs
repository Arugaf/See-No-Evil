using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Features.VFX
{
    public class SmoothDampArticulator
    {
        public float Current { get; private set; }
        public float Target;
        private float speed;
        private float smoothTime;
        private float maxSpeed;
        public SmoothDampArticulator(float currVal, float smoothTime, float maxSpeed = float.MaxValue)
        {
            Current = currVal;
            Target = currVal;
            this.smoothTime = smoothTime;
            this.maxSpeed = maxSpeed;
        }
        public void Update() => Update(Time.deltaTime);
        public void Update(float dt)
        {
            Current = Mathf.SmoothDamp(Current, Target, ref speed, smoothTime, maxSpeed, dt);
        }
    }
    public class SmoothDampArticulatorToMultiplier
    {
        public float Current { get; private set; }
        public float TargetRatio { get => target / maxVal; set => target = value * maxVal; }
        private float target;
        private float maxVal;
        private float speed;
        private float smoothTime;
        private float maxSpeed;
        public SmoothDampArticulatorToMultiplier(float maxVal, float smoothTime, float maxSpeed = float.MaxValue)
        {
            this.maxVal = maxVal;
            target = maxVal;
            Current = target;
            this.smoothTime = smoothTime;
            this.maxSpeed = maxSpeed;
        }
        public void Update() => Update(Time.deltaTime);
        public void Update(float dt)
        {
            Current = Mathf.SmoothDamp(Current, target, ref speed, smoothTime, maxSpeed, dt);
        }
    }
}

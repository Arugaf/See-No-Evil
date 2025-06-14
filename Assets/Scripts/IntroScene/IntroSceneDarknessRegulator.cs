using Features.VFX;
using UnityEngine;
namespace Features.IntroScene
{
    // Diabolical type shit duplicating the singleton :skull:
    public class IntroSceneDarknessRegulator: MonoBehaviour
    {
        private SmoothDampArticulator articulator;
        [SerializeField] private float smoothTime;
        
        private void Start()
        {
            articulator = new SmoothDampArticulator(0, smoothTime);
        }

        private void Update()
        {
            articulator.Update();
            Shader.SetGlobalFloat(DarknessManager.DARKNESS_FACTOR, articulator.Current);
        }
        public void SetDarknessFactor(float fac)
        {
            articulator.Target = fac;
        }
    }
}

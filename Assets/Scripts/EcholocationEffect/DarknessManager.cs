using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.VFX
{
    public class DarknessManager : MonoBehaviour
    {
        public const string DARKNESS_FACTOR = "DARKNESS_FACTOR";
        private const float DARKNESS_MAX_STATE = 0.95f;
        public static float DarknessFactor { get; private set; }
        public static bool ShowDarknessObjects => DarknessFactor > DARKNESS_MAX_STATE;

        [SerializeField] private InputActionAsset asset;
        [SerializeField] private float smoothTime;
        private InputAction act;

        private SmoothDampArticulator articulator;
        private void Awake()
        {
            articulator = new SmoothDampArticulator(1, smoothTime);
            DarknessFactor = 1;
            Shader.SetGlobalFloat(DARKNESS_FACTOR, DarknessFactor);
            act = asset.FindAction("Attack");
        }
        private void OnDestroy()
        {
            SetDarknessFactor(0);
        }
        private void Update()
        {
            articulator.Target = act.IsPressed() ? 1.0f : 0.0f;
            SetDarknessFactor(articulator.Current);
            articulator.Update();
        }
        private void SetDarknessFactor(float fac)
        {
            DarknessFactor = fac;
            Shader.SetGlobalFloat(DARKNESS_FACTOR, DarknessFactor);
        }
    }
}

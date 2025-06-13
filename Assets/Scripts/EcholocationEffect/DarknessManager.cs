using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.VFX
{
    public class DarknessManager : MonoBehaviour
    {
        private const string DARKNESS_FACTOR = "DARKNESS_FACTOR";
        private const float DARKNESS_MAX_STATE = 0.95f;
        public static float DarknessFactor { get; private set; }
        public static bool ShowDarknessObjects => DarknessFactor > DARKNESS_MAX_STATE;

        [SerializeField] private InputActionAsset asset;
        [SerializeField] private float smoothTime;
        private InputAction act;

        private float currentVelocity;
        private void Awake()
        {
            DarknessFactor = 0;
            Shader.SetGlobalFloat(DARKNESS_FACTOR, DarknessFactor);
            act = asset.FindAction("Crouch");
        }
        private void OnDestroy()
        {
            SetDarknessFactor(0);
        }
        private void Update()
        {
            SetDarknessFactor(Mathf.SmoothDamp(DarknessFactor, act.IsPressed() ? 1.0f: 0.0f, ref currentVelocity, smoothTime));
        }
        private void SetDarknessFactor(float fac)
        {
            DarknessFactor = fac;
            Shader.SetGlobalFloat(DARKNESS_FACTOR, DarknessFactor);
        }
    }
}

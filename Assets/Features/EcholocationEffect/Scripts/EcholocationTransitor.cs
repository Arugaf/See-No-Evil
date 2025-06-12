using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.VFX
{
    public class EcholocationTransitor : MonoBehaviour
    {
        [SerializeField] private InputActionAsset asset;
        [SerializeField] private float smoothTime;
        [SerializeField] private InputAction act;
        private float darknessFactor;
        private float currentVelocity;
        private void Awake()
        {
            act = asset.FindAction("Crouch");
        }
        private void Update()
        {
            darknessFactor = Mathf.SmoothDamp(darknessFactor, act.IsPressed() ? 1.0f: 0.0f, ref currentVelocity, smoothTime);
            Shader.SetGlobalFloat("DARKNESS_FACTOR", darknessFactor);
        }
    }
}

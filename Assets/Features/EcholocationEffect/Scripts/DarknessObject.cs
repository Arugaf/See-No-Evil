using UnityEngine;

namespace Features.VFX
{
    [RequireComponent(typeof(MeshRenderer))]
    public class DarknessObject : MonoBehaviour
    {
        private MeshRenderer _renderer;
        private void Awake()
        {
            _renderer = GetComponent<MeshRenderer>();
        }
        private void Update()
        {
            _renderer.enabled = DarknessManager.ShowDarknessObjects;
        }
    }
}

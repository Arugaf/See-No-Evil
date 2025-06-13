using Features.AudioManager;
using UnityEngine;
public class FootstepAudioPlayer : MonoBehaviour
{
    [SerializeField] private float distanceToGround;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private AudioMaterialHolder defaultAudioMaterial;
    public void Play()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit info, distanceToGround, layerMask))
        {
            AudioPlayDeterminedParams? param = null;
            if (info.collider.gameObject.TryGetComponent(out IAudioMaterialHolder holder))
            {
                param = holder.Generate();
            }
            else param = defaultAudioMaterial?.Generate();
            if(param != null)
            {
                AudioManager.PlayAtomic(transform.position, param);
            }
        }

    }
}

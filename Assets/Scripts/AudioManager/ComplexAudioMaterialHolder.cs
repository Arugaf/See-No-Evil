using Features.AudioManager;
using UnityEngine;

public class ComplexAudioMaterialHolder :MonoBehaviour, IAudioMaterialHolder
{
    [SerializeField] private AudioMaterialHolder[] holders;
    [SerializeField] private AudioStepMaterial defaultMaterial;
    [SerializeField] private float critDistance = 2.0f;
    private Collider[] colliders;
    public void Awake()
    {
        colliders = new Collider[holders.Length];
        for (int i = 0; i < holders.Length; i++)
        {
            colliders[i] = holders[i].GetComponent<Collider>();
        }
    }
    public AudioPlayDeterminedParams RetrieveAt(Vector3 point)
    {
        for (int i = 0; i < holders.Length; i++)
        {
            Vector3 dist = colliders[i].ClosestPoint(point) - point;
            if (dist.magnitude < critDistance)
            {
                return holders[i].RetrieveAt(point);
            }
        }
        return defaultMaterial?.Generate();
    }
}

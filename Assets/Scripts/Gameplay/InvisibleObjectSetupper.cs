using UnityEngine;
public class InvisibleObjectSetupper : MonoBehaviour
{
    private const bool ENABLE_INVISIBILITY = false;
    [SerializeField] private Material normalMaterial;
    private void Awake()
    {
        var m = GetComponent<Renderer>();
        if (m.material != normalMaterial && !ENABLE_INVISIBILITY)
        {
            m.material = normalMaterial;
        }
    }
}

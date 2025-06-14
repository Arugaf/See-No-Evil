using Actors;
using UnityEngine;

public class KillIfTooLow : MonoBehaviour
{
    private Health hp;
    [SerializeField] private float criticalYToKill = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Update is called once per frame
    private void Start()
    {
        hp = GetComponent<Health>();
    }
    void Update()
    {
        if(hp.transform.position.y < criticalYToKill)
        {
            hp.DoDamage(999999); // kys
            enabled = false;
        }
    }
}

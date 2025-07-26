using System;
using UnityEngine;

public class SimpleHealingItemAnimator : MonoBehaviour
{
    [SerializeField] private Vector3 verticalAcc;
    [SerializeField] private float period;
    [SerializeField] private float rotationPeriod;
    [SerializeField] private float rotationAmplitude;
    [SerializeField] private Transform targetTransorm;

    private bool isVisible = true;

    // Update is called once per frame
    void Update()
    {
        if (isVisible)
        {
            float x = Mathf.Sin(Time.time * period);
            float rot = Mathf.Sin(Time.time * rotationPeriod);
            targetTransorm.localPosition = x * verticalAcc;
            targetTransorm.localEulerAngles = new Vector3(0, rot * rotationAmplitude, 0);
        }
    }
    void OnBecameVisible()
    {
        isVisible = true;
    }
    void OnBecameInvisible()
    {
        isVisible = false;        
    }
}

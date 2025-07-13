using Cysharp.Threading.Tasks;
using Features.VFX;
using UnityEngine;

public abstract class AbstractAmbushMonsterBehaviour: MonoBehaviour
{
    public abstract UniTask DoAmbush(Transform player);
}
public class AmbushMonsterBehaviour : AbstractAmbushMonsterBehaviour
{
    [SerializeField] private Transform[] targetPoints;
    [SerializeField] private float speed;
    [SerializeField] private float rotationSmoothSpeed;
    [SerializeField] private float velocitySmoothTime = 0.2f;
    [SerializeField] private float critDistance = 0.2f;
    public override async UniTask DoAmbush(Transform target)
    {
        gameObject.SetActive(true);
        transform.position = targetPoints[0].position;
        int i = 0;
        Vector3 acc = Vector3.zero, velocity = Vector3.zero;
        while (i < targetPoints.Length)
        {
            float tFactor = Mathf.Exp(rotationSmoothSpeed * Time.deltaTime) - 1;
            Vector3 moveVector = targetPoints[i].position - transform.position;
            Quaternion rot = Quaternion.LookRotation(moveVector.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, tFactor);
            Vector3 targetVelocity = moveVector.normalized * speed;
            velocity = Vector3.SmoothDamp(velocity, targetVelocity, ref acc, velocitySmoothTime);
            transform.position = transform.position + velocity * Time.deltaTime;
            await UniTask.WaitForEndOfFrame();
            if (moveVector.magnitude < critDistance) i++;
        }
        gameObject.SetActive(false);
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.coral;

        for (var i = 0; i < targetPoints.Length - 1; ++i)
        {
            Gizmos.DrawLine(targetPoints[i].position, targetPoints[i + 1].position);
        }

    }
#endif
}

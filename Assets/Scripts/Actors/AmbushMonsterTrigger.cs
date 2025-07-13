using Cysharp.Threading.Tasks;
using UnityEngine;
namespace Gameplay
{
    [RequireComponent(typeof(Collider))]
    public class AmbushMonsterTrigger : MonoBehaviour
    {
        private bool canTrigger = true;
        [SerializeField] private float determTag = 0.3f;
        [SerializeField] private float cooldown = 10.0f;
        [SerializeField] private GameObject activateObject;
        [SerializeField] private AbstractAmbushMonsterBehaviour ambushBehaviour;
        private void OnTriggerEnter(Collider other)
        {
            if (!canTrigger) return;
            if (other.CompareTag("Player"))
            {
                canTrigger = false;
                TriggerProcess(other.transform).Forget();
            }
        }
        private async UniTask TriggerProcess(Transform player)
        {
            Vector3 startPos = player.position;
            await UniTask.WaitForSeconds(determTag);
            Vector3 forwarder = player.position - startPos;
            float perform = Vector3.Dot(forwarder, transform.forward);
            if (perform <= 0)
            {
                canTrigger = true;
                return;
            }
            else
            {
                Debug.Log("DO AMBUSH");
                await ambushBehaviour.DoAmbush(player);
                canTrigger = true;
            }
        }
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Debug.DrawRay(transform.position, transform.forward * 2, Color.yellow);
        }
#endif
    }
}
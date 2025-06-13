using UnityEngine;

namespace Actors {
    public class DamageDealer : MonoBehaviour {
        [SerializeField] private int damage = 25;
        [SerializeField] private float radius = 5f;
        
        public void Attack(Transform target) {
            if (!target || Vector3.Distance(transform.position, target.position) > radius) return;

            var targetHealth = target.GetComponent<Health>();
            
            if (!targetHealth) return;
            
            targetHealth.ApplyHpChange(damage);
        }
    }
}

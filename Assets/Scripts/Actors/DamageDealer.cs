using System;
using UnityEngine;

namespace Actors {
    public class DamageDealer : MonoBehaviour {
        [SerializeField] private int damage = 25;
        [SerializeField] private float radius = 5f;

        [SerializeField] private Color radiusSphereColor = Color.mediumAquamarine;
        [SerializeField] private Color radiusSphereColorTriggered = Color.crimson;

        [SerializeField] private Transform debugTriggerTarget;
        [SerializeField] private float currentDistance;

        public void Attack(Transform target) {
            if (!target || Vector3.Distance(transform.position, target.position) > radius) return;

            var targetHealth = target.GetComponent<Health>();

            if (!targetHealth) return;

            targetHealth.ApplyHpChange(damage);
        }

        private void OnDrawGizmos() {
            if (debugTriggerTarget) {
                currentDistance = Vector3.Distance(transform.position, debugTriggerTarget.position);

                Gizmos.color = currentDistance > radius ? radiusSphereColor : radiusSphereColorTriggered;
            }

            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}

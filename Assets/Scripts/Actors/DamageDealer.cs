using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

namespace Actors {
    public class DamageDealer : MonoBehaviour {
        [SerializeField] private DamageParameters damage; 
        [field: SerializeField] public float AttackRadius { get; private set; } = 5f;
        [SerializeField] private float deltaTime;
        [SerializeField] private UnityEvent OnAttackTriggeredByAnimator;
#if UNITY_EDITOR
        [SerializeField] private bool gizmosActive = true;
        [SerializeField] private Color radiusSphereColor = Color.mediumAquamarine;
        [SerializeField] private Color radiusSphereColorTriggered = Color.crimson;

        [SerializeField] private Transform debugTriggerTarget;
        [SerializeField] private float currentDistance;

#endif
        public bool Attacking { get; private set; }
        private Transform currentAttackingTransorm;
        public void DoAttack(Transform attacking)
        {
            if (attacking != null && !Attacking)
            {
                Attacking = true;
                currentAttackingTransorm = attacking;
            }
            else if(attacking == null && Attacking)
            {
                Attacking = false;
                currentAttackingTransorm = null;
            }
        }
        // Animator triggers it (in order to achieve animator sync)
        public void TriggerAttack()
        {
            if(Vector3.Distance(transform.position, currentAttackingTransorm.transform.position) > AttackRadius)
            {
                Attacking = false;
            }
            else if(currentAttackingTransorm.TryGetComponent<IDamageable>(out var currentAttacking)) 
            {
                currentAttacking.Damage(damage);
                OnAttackTriggeredByAnimator?.Invoke();
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos() {
            if (!gizmosActive) return;
            
            if (debugTriggerTarget) {
                currentDistance = Vector3.Distance(transform.position, debugTriggerTarget.position);

                Gizmos.color = currentDistance > AttackRadius ? radiusSphereColor : radiusSphereColorTriggered;
            }

            Gizmos.DrawWireSphere(transform.position, AttackRadius);
        }
#endif
    }
}

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
        [SerializeField] private UnityEvent OnEachAttack;
#if UNITY_EDITOR
        [SerializeField] private bool gizmosActive = true;
        [SerializeField] private Color radiusSphereColor = Color.mediumAquamarine;
        [SerializeField] private Color radiusSphereColorTriggered = Color.crimson;

        [SerializeField] private Transform debugTriggerTarget;
        [SerializeField] private float currentDistance;

#endif
        public bool Attacking { get; private set; }
        private Coroutine currentAttack;
        public void DoAttack(Transform attacking)
        {
            if (attacking != null && !Attacking)
            {
                Attacking = true;
                currentAttack = StartCoroutine(DoAttackCoroutine(attacking));
            }
            else if(attacking == null && Attacking)
            {
                Attacking = false;
                StopCoroutine(currentAttack);
                currentAttack = null;
            }
        }
        private IEnumerator DoAttackCoroutine(Transform t)
        {
            IDamageable hp = t.GetComponent<IDamageable>();
            if (hp == null)
            {
                Attacking = false;
                yield break;
            }
            while (true)
            {
                OnEachAttack?.Invoke();
                yield return new WaitForSeconds(deltaTime);
                if (Vector3.Distance(transform.position, t.transform.position) > AttackRadius)
                {
                    Attacking = false;
                    yield break;
                }
                hp.Damage(damage);
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

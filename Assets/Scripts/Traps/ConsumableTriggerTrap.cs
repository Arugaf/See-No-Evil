using Actors;
using UnityEngine;
using UnityEngine.Events;
namespace Traps
{
    public class ConsumableTriggerTrap: MonoBehaviour
    {
        [SerializeField] private GameObject spawnEffect;
        public DamageParameters damageParameters;
        public UnityEvent OnAttackAction;
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                Destroy(gameObject);
                if(spawnEffect != null) Instantiate(spawnEffect, transform.position, transform.rotation);
                damageable.Damage(damageParameters);
                OnAttackAction?.Invoke();
            }
        }
    }
}

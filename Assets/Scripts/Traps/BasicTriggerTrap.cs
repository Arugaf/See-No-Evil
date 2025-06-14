using Actors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Traps
{
    public class BasicTriggerTrap: MonoBehaviour
    {
        public DamageParameters damageParameters;
        public float cooldown;
        public UnityEvent OnAttackAction;
        private bool triggerDisabled = false;
        private void OnTriggerEnter(Collider other)
        {
            if (triggerDisabled) return;
            if (other.TryGetComponent(out IDamageable damageable))
            {
                triggerDisabled = true;
                StartCoroutine(CooldownCoroutine());
                damageable.Damage(damageParameters);
                OnAttackAction?.Invoke();
            }
        } 
        private IEnumerator CooldownCoroutine()
        {
            yield return new WaitForSeconds(cooldown);
            triggerDisabled = false;
        }
    }
}

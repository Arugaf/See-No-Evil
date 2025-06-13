using Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Traps
{
    public class BasicTriggerTrap: MonoBehaviour
    {
        public DamageParameters damageParameters;
        public void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                damageable.Damage(damageParameters);
            }
        }
    }
}

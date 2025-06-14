using KinematicCharacterController.Examples;
using UnityEngine;
using Actors;
using System.Collections;
namespace Actors
{
    public class PlayerDamageReactor : MonoBehaviour, IDamageable
    {
        public PlayerMovementParametersManager playerMovementParametersManager;
        public Health health;
        private Coroutine stunCoroutine = null;
        public void Damage(in DamageParameters parameters)
        {
            health.DoDamage(parameters.Damage);
            playerMovementParametersManager.SetStunCoeff(parameters.StunCoeff);
            if (stunCoroutine != null) StopCoroutine(stunCoroutine);
            stunCoroutine = StartCoroutine(Stun(parameters));
        }
        private IEnumerator Stun(DamageParameters parameters)
        {
            playerMovementParametersManager.SetStunCoeff(parameters.StunCoeff);
            yield return new WaitForSeconds(parameters.StunTime);
            playerMovementParametersManager.SetStunCoeff(1.0f);
        }
    }
}
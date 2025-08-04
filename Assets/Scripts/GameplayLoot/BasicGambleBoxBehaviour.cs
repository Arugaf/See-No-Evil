using Features.AudioManager;
using UnityEngine;
namespace Gameplay.Loot
{
    public class BasicGambleBoxBehaviour: GambleBoxBehaviour
    {
        [SerializeField] private AudioStepMaterial onPickupSound;
        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PickUp();
                enabled = false;
                Destroy(gameObject);
                AudioManager.PlayAtomic(transform.position, onPickupSound.Generate());
                return;
            }
        }
    }
}

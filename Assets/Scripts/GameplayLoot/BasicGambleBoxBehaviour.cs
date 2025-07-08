using UnityEngine;
namespace Gameplay.Loot
{
    public class BasicGambleBoxBehaviour: GambleBoxBehaviour
    {
        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PickUp();
                enabled = false;
                Destroy(gameObject);
                return;
            }
        }
    }
}

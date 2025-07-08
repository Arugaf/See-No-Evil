using UnityEngine;
using UnityEngine.Events;
namespace Gameplay.Loot
{
    public abstract class GambleBoxBehaviour: MonoBehaviour
    {
        public event UnityAction OnBeingPickedUp;
        protected void PickUp() => OnBeingPickedUp?.Invoke();
    }
}

using Gameplay;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;
namespace UI
{
    public class LookaroundField : MonoBehaviour, IDragHandler
    {
        public MobileGameplayUIView mobileGameplayUIView;
        private bool dragOnUpdate = false;
        public void OnDrag(PointerEventData eventData)
        {
            mobileGameplayUIView.CurrentLookVector = eventData.delta;
            dragOnUpdate = true;
        }
        public void LateUpdate()
        {
            if (dragOnUpdate)
            {
                dragOnUpdate = false;
            }
            // one-frame delay so the Tutorial might read CurrentLookVector value. yes, this solution sucks ass
            else if (mobileGameplayUIView.CurrentLookVector.sqrMagnitude > 0.0001f)
            {
                mobileGameplayUIView.CurrentLookVector = Vector2.zero;
            }
        }
    }
}
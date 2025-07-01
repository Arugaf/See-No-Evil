using Gameplay;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;
namespace UI
{ 
    public class LookaroundField : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        public MobileGameplayUIView mobileGameplayUIView;
        public void OnDrag(PointerEventData eventData)
        {
            mobileGameplayUIView.CurrentLookVector = eventData.delta;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            mobileGameplayUIView.CurrentLookVector = Vector2.zero;
        }
    }
}
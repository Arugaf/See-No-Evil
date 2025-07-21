using Features.OutroScene;
using UnityEngine;
using UnityEngine.EventSystems;

public class GamblingItemRotator : MonoBehaviour, IDragHandler
{
    [SerializeField] private GamblingItemView itemView;
    [SerializeField] private float frequency = 180;
    public void OnDrag(PointerEventData eventData)
    {
        itemView.Rotate(-frequency * eventData.delta.x / Screen.width);

    }
}

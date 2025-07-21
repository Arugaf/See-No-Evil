using UnityEngine;

namespace Features.OutroScene
{
    public interface IGambleBoxView
    {
        public void SetOpen(bool isOpen);
    }
    public class SimpleGambleBoxView : MonoBehaviour, IGambleBoxView
    {
        public void SetOpen(bool isOpen) => gameObject.SetActive(!isOpen);
    }
}
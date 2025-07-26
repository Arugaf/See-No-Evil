using UnityEngine;

namespace Features.OutroScene
{
    public class AnimatedGambleBoxView : MonoBehaviour, IGambleBoxView
    {
        [SerializeField] private Animator anim;
        public void SetOpen(bool isOpen)
        {
            anim.SetBool("Open", isOpen);
        }
    }
}
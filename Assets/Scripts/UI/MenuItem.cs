using Features.IntroScene;
using UnityEngine;
namespace UI
{
    public class MenuItem: MonoBehaviour
    {
        [SerializeField] private Animator anim;
        [SerializeField] private LegacyIntroCameraController controller;
        public void SetEnabled(bool enabled)
        {
            anim.SetBool("Show", enabled);
            controller?.SetInteractableState(!enabled);
        }
    }
}

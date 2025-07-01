using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
namespace Gameplay 
{
    public class DarknessMeterView : MonoBehaviour
    {
        [SerializeField] private List<Slider> slidersToUpdate;
        [SerializeField] private Animator barAnimator;
        private DarknessMeterController meterController;
        [Inject]
        private void Construct(DarknessMeterController controller)
        {
            meterController = controller;
        }
        private void Update()
        {
            foreach(var s in slidersToUpdate)
            {
                s.value = meterController.Ratio;
            }
            barAnimator.SetBool("Decaying", meterController.DoDecay);
            barAnimator.SetFloat("Ratio", meterController.Ratio);
        }
    } 
}

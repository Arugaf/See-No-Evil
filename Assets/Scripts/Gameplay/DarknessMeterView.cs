using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Gameplay 
{
    public class DarknessMeterView : MonoBehaviour
    {
        [SerializeField] private List<Slider> slidersToUpdate;
        [SerializeField] private DarknessMeterController meterController;
        [SerializeField] private Animator barAnimator;
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

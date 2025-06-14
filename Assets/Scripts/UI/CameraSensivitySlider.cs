using KinematicCharacterController.Examples;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;
namespace UI
{
    [RequireComponent(typeof(Slider))]
    public class CameraSensivitySlider: MonoBehaviour
    {
        private Slider slider;
        private void Awake()
        {
            slider = GetComponent<Slider>();
            slider.value = ExamplePlayer.PlayerCameraSensivityCoeff;
            slider.onValueChanged.AddListener(Sync);
        }
        private void Sync(float ratio)
        {
            ExamplePlayer.PlayerCameraSensivityCoeff = ratio;
        }
    }
}
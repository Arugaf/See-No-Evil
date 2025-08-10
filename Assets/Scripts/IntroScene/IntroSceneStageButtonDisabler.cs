using UnityEngine;
using UnityEngine.UI;
namespace Features.IntroScene
{
    public class IntroSceneStageButtonDisabler: IntroSceneStageSubcomponent
    {
        private Button[] allButtons;
        [SerializeField] private Transform buttonHolder;
        private void Awake()
        {
            allButtons = buttonHolder.GetComponentsInChildren<Button>();
        }
        public override void IntentionForActivation(bool state)
        {
            if (allButtons == null) Awake();
            foreach (var button in allButtons)
            {
                button.enabled = state;
            }
        }
    }
}
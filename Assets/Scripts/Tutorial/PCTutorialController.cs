using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using VContainer;
namespace Tutorial
{
    public class PCTutorialController : BaseTutorialController
    {
        [SerializeField] private LocalizedString moveHelpingLabel;
        [SerializeField] private LocalizedString lookHelpingLabel;
        [SerializeField] private LocalizedString tapHelpingLabel;
        [SerializeField] private LocalizedString allSettledLabel;
        private InputActionAsset asset;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        [Inject]
        private void Construct(InputActionAsset asset)
        {
            this.asset = asset;
        }
        protected override ITutorialSection GetTutorialSection()
        {
            return new CompositeTutorialSection(
                new InputActionTutorialSection(moveHelpingLabel, asset.FindAction("Move")),
                new InputActionTutorialSection(lookHelpingLabel, asset.FindAction("Look")),
                new InputActionTutorialSection(tapHelpingLabel, asset.FindAction("Attack"), 0.0f),
                new DullSection(allSettledLabel, 3.0f)
            );
        }
    }
}
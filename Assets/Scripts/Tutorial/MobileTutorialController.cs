using Gameplay;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using VContainer;
namespace Tutorial
{
    public class MobileViewAction : ContiguousActionTutorialSection
    {
        MobileGameplayUIView view;
        public MobileViewAction(LocalizedString loc, MobileGameplayUIView view, float time = 1) : base(loc, time)
        {
            this.view = view;
        }

        protected override bool IsProgressing()
        {
            return view.CurrentLookVector.magnitude > 0.1f;
        }
    }
    public class MobileTutorialController : BaseTutorialController
    {
        [SerializeField] private MobileGameplayUIView mobileGameplayUIView;
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
                new MobileViewAction(lookHelpingLabel, mobileGameplayUIView),
                new InputActionTutorialSection(tapHelpingLabel, asset.FindAction("Attack"), 0.0f),
                new DullSection(allSettledLabel, 3.0f)
            );
        }
    }
}
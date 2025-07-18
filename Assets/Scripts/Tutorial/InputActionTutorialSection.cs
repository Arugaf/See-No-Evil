using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
namespace Tutorial
{
    public abstract class ContiguousActionTutorialSection : LocalizedTutorialSection
    {
        private float lTime;
        public ContiguousActionTutorialSection(LocalizedString loc, float time = 1.0f) : base(loc)
        {
            lTime = time;
        }
        protected sealed override async UniTask DoPerform(ITutorialView view)
        {
            float progress = 0;
            view.Progress = 0;
            while (progress < 1)
            {
                if (IsProgressing())
                {
                    if (lTime == 0) progress = 1;
                    else
                    {
                        progress += Time.deltaTime / lTime;
                    }
                    view.Progress = progress;
                }
                await UniTask.WaitForEndOfFrame();
            }
            await view.DoLogicalBreak();
        }
        protected abstract bool IsProgressing();
    }
    public class InputActionTutorialSection : ContiguousActionTutorialSection
    {
        private InputAction reference;

        public InputActionTutorialSection(LocalizedString loc, InputAction act, float time = 1) : base(loc, time)
        {
            reference = act;
        }

        protected override bool IsProgressing() => reference.IsPressed();
    }
}
using Cysharp.Threading.Tasks;
using UnityEngine.Localization;
namespace Tutorial
{
    // It does nothing but waits.
    public class DullSection : LocalizedTutorialSection
    {
        private float seconds;
        private float progress = 1;
        public DullSection(LocalizedString loc, float seconds, float progress = 1) : base(loc)
        {
            this.progress = 1;
            this.seconds = seconds;
        }

        protected override async UniTask DoPerform(ITutorialView view)
        {
            view.Progress = progress;
            await UniTask.WaitForSeconds(seconds);
        }
    }
}
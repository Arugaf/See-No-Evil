using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization;
namespace Tutorial
{
    public abstract class LocalizedTutorialSection : ITutorialSection
    {

        private LocalizedString localizedString;
        public LocalizedTutorialSection(LocalizedString loc)
        {
            localizedString = loc;
        }
        public async UniTask Perform(ITutorialView view)
        {
            view.Caption = await localizedString.GetLocalizedStringAsync();
            LocalizedString.ChangeHandler change = (string x) => view.Caption = x;
            localizedString.StringChanged += change;
            await DoPerform(view);
            localizedString.StringChanged -= change;
        }
        protected abstract UniTask DoPerform(ITutorialView view);
    }
}
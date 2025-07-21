using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
namespace Features.OutroScene
{
    [Serializable]
    public class LocalizedTextController
    {
        [SerializeField] private TextMeshProUGUI text;
        private LocalizedString current;

        public LocalizedTextController(TextMeshProUGUI text)
        {
            this.text = text;
        }

        public async UniTask SetText(LocalizedString localizedString)
        {
            if (current != null)
            {
                current.StringChanged -= StringChanged;
            }
            current = localizedString;
            current.StringChanged += StringChanged;
            StringChanged(await localizedString.GetLocalizedStringAsync());
        }
        public void StringChanged(string s) => text.text = s;

    }
}
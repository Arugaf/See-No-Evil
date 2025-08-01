using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
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
        public void Reset(string value = "")
        {
            if (current != null)
            {
                current.StringChanged -= StringChanged;
            }
            current = null;
            text.text = value;
        }
        public async UniTask SetText(LocalizedString localizedString, Dictionary<string, string> keyVals = null)
        {
            if (current != null)
            {
                current.StringChanged -= StringChanged;
            }
            current = localizedString;
            if (keyVals != null)
            {
                current.Values.Clear();
                foreach (var p in keyVals)
                {
                    current.Add(p.Key, new StringVariable { Value = p.Value });
                }
            }
            StringChanged(await localizedString.GetLocalizedStringAsync());
            current.StringChanged += StringChanged;
        }
        public void StringChanged(string s) => text.text = s;

    }
}
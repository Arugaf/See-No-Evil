using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization;
namespace Levels
{
    public class UndiscoveredLevelListItem : DullLevelListItem
    {
        private LocalizedString stat;

        public UndiscoveredLevelListItem(GameLevelInfoObject obj, LocalizedString stat, bool isUnlocked, bool isSelected) : base(obj, isUnlocked, isSelected)
        {
            this.stat = stat;
        }

        public override async UniTask<string> GetStatDescription() => await stat.GetLocalizedStringAsync();
    }
}
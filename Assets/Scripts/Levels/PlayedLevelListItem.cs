using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Gameplay;
using UnityEngine;
using UnityEngine.Localization;
namespace Levels
{
    public class PlayedLevelListItem : DullLevelListItem
    {
        private float time;
        private int score;
        private LocalizedString stat;
        public PlayedLevelListItem(GameLevelInfoObject obj, LocalizedString stat, float time, int score, bool isMain) : base(obj, true, isMain)
        {
            this.stat = stat;
            this.time = time;
            this.score = score;
        }
        public override async UniTask<string> GetStatDescription()
        {
            var arguments = new Dictionary<string, string> { { "Time", GameplayResultStorage.GetTimeSpec(time) },
                                                             { "Score", score.ToString()} };
            return await stat.GetLocalizedStringAsync(arguments);
        }
    }
}
using Cysharp.Threading.Tasks;
using UnityEngine;
namespace Tutorial
{
    public abstract class BaseTutorialView : MonoBehaviour, ITutorialView
    {
        public abstract string Caption { set; }
        public abstract float Progress { set; }

        public virtual UniTask DoLogicalBreak() => UniTask.CompletedTask;
        public virtual UniTask Hide() => UniTask.CompletedTask;
        public virtual UniTask Show() => UniTask.CompletedTask;
    }
}
using Cysharp.Threading.Tasks;

namespace Tutorial
{
    public interface ITutorialView
    {
        public string Caption { set; }
        public float Progress { set; }
        public UniTask Show();
        // Section delay (like when)
        public UniTask DoLogicalBreak();
        public UniTask Hide();
    }
}
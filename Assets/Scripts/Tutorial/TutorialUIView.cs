using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
namespace Tutorial
{
    public class TutorialUIView : BaseTutorialView
    {
        public override string Caption { set => text.text = value; }
        public override float Progress { set => SetProgress(value); }
        private float progress;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private float changePeriod;
        [SerializeField] private Gradient progressColorFirst;
        [SerializeField] private Gradient progressColorSecond;
        [SerializeField] private Animator mainAnimator;
        [SerializeField] private float showHideDuration;
        [SerializeField] private float logicalBreakDuration;
        private Color top, bottom;
        private void SetProgress(float value)
        {
            progress = value;
            top = progressColorFirst.Evaluate(progress);
            bottom = progressColorSecond.Evaluate(progress);
        }
        private void Update()
        {
            float s = Mathf.Cos(Time.time * changePeriod);
            text.color = Color.Lerp(top, bottom, s * s);
        }
        public override UniTask Show()
        {
            mainAnimator.SetBool("Hide", false);
            return UniTask.CompletedTask;//UniTask.WaitForSeconds(showHideDuration / 3.0f);
        }
        public override UniTask DoLogicalBreak()
        {
            mainAnimator.SetTrigger("Success");
            return UniTask.WaitForSeconds(logicalBreakDuration);
        }
        public override UniTask Hide()
        {
            mainAnimator.SetBool("Hide", true);
            return UniTask.WaitForSeconds(showHideDuration);
        }
    }
}

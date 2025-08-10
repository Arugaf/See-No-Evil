using Cysharp.Threading.Tasks;
using YG;
using System;
namespace External
{
    public class PluginYGReviewOpener : IReviewOpener
    {
        public bool IsAvailable => YG2.reviewCanShow;

        public UniTask<bool> OpenReview()
        {
            var x = new UniTaskCompletionSource<bool>();
            Action<bool> deleg = null;
            deleg = (bool dat) =>
            {
                x.TrySetResult(dat);
                YG2.onReviewSent -= deleg;
            };
            YG2.onReviewSent += deleg;
            YG2.ReviewShow();
            return x.Task;
        }
    }
}

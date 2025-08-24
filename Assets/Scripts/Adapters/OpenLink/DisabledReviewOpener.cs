using Cysharp.Threading.Tasks;
namespace External
{
    public class DisabledReviewOpener : IReviewOpener
    {
        public bool IsAvailable => false;

        public UniTask<bool> OpenReview()
        {
            return UniTask.FromResult(false);
        }
    }
}

using UnityEngine;
using Cysharp.Threading.Tasks;
namespace External
{
    public interface ITemporaryAvailable
    {
        public bool IsAvailable { get; }
    }
    public interface ILinkOpener: ITemporaryAvailable
    {
        public UniTask OpenLink(string link);
    }
    public interface IReviewOpener: ITemporaryAvailable
    {
        public UniTask<bool> OpenReview();
    }
    public interface IAddAsLinkButton: ITemporaryAvailable
    {
        public UniTask AddAsLink();
    }
}

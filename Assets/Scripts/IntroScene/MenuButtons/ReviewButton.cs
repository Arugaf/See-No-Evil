using Cysharp.Threading.Tasks;
using External;
using UnityEngine;

public class ReviewButton: GenericLinkButton<IReviewOpener>
{
    protected override bool TryHidingOnDone => false;
    [SerializeField] private GameObject onReviewSuccess;
    [SerializeField] private GameObject onReviewBasic;
    protected override async UniTask Action()
    {
        bool result = await service.OpenReview();
        onReviewBasic.SetActive(!result);
        onReviewSuccess.SetActive(result);
    }
}
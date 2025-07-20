using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
namespace Monetization
{
    public class AdShowResult
    {
        public enum Status { Success, Failure, Cancelled}
        public Status ShowStatus { get; private set; }
        public bool IsSuccess => ShowStatus == Status.Success;
        public AdShowResult(Status status)
        {
            this.ShowStatus = status;
        }
        public static implicit operator AdShowResult(Status status) => new AdShowResult(status);
    }
    public interface IAdManager : IDisposable
    {
        public UniTask PreloadAdvertisement();
        public UniTask<AdShowResult> ShowAdvertisement();
        public UniTask<AdShowResult> ShowRewardedAdverticement();
    }
}
using UnityEngine;
namespace External
{
    public interface IGameReporter
    {
        public void GameIsReadyAndInteractable();
        public bool IsPlaying { get; set; }
    }
}
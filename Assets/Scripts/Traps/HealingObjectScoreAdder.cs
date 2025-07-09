using Gameplay;
using UnityEngine;
using VContainer;

public class HealingObjectScoreAdder : MonoBehaviour
{
    private IGameplayScoreManager gameplayScoreManager;
    [Inject]
    private void Construct(IGameplayScoreManager gameplayScoreManager) => this.gameplayScoreManager = gameplayScoreManager;
    public void AddScore() => gameplayScoreManager?.CollectedItemAdded();
}

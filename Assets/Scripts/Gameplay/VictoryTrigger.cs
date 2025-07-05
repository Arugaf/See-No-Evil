using UnityEngine;
using VContainer;

namespace Gameplay {
    [RequireComponent(typeof(Collider))]

    public class VictoryTrigger : MonoBehaviour 
    {
        private GameplayState gameplayState;
        [Inject]
        private void Construct(GameplayState gameplayState)
        {
            this.gameplayState = gameplayState;
        }
        private void OnTriggerEnter(Collider other) {
            if (gameplayState == null) return;
            
            Debug.Log("Collision triggered");
            
            if (other.CompareTag("Player")) {
                gameplayState.Victory();
            }
        }
    }
}

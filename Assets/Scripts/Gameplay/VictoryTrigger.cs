using UnityEngine;

namespace Gameplay {
    [RequireComponent(typeof(Collider))]

    public class VictoryTrigger : MonoBehaviour {
        [SerializeField] private GameplayState gameplayState;
        
        private void OnTriggerEnter(Collider other) {
            if (!gameplayState) return;
            
            Debug.Log("Collision triggered");
            
            if (other.CompareTag("Player")) {
                gameplayState.Victory();
            }
        }
    }
}

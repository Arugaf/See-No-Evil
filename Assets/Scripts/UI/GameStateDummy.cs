using UnityEngine;

namespace UI {
    public class GameStateDummy : MonoBehaviour {
        private GameStateManager _gameStateManager;

        void Start() {
            _gameStateManager = FindFirstObjectByType<GameStateManager>();
        }
        
        public void LoadGame() {
            _gameStateManager.LoadGame();
        }

        public void LoadMenu() {
            _gameStateManager.LoadMenu();
        }

        public void LoadGameOverScene() {
            _gameStateManager.LoadGameOverScene();
        }
    }
}

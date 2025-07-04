using UnityEngine;

namespace UI {
    public class GameStateDummy : MonoBehaviour 
    {
        
        public void LoadGame() {
            GameStateManager.LoadGameScene();
        }

        public void LoadMenu() {
            GameStateManager.LoadIntroScene();
        }

        public void LoadGameOverScene() {
            GameStateManager.LoadGameOver();
        }
    }
}

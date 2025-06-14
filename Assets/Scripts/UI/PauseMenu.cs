using UnityEngine;

namespace UI {
    public class PauseMenu : MonoBehaviour {
        private static PauseMenu _instance = null;
        [SerializeField] GameObject MenuObject;
        private void Awake() {
            DontDestroyOnLoad(gameObject);

            if (!_instance) {
                _instance = this;
            }
            else if (_instance != this) {
                Destroy(gameObject);
            }
        }
        
        public static void SetState(bool shown)
        {
            _instance?.MenuObject.SetActive(shown);
        }
        public void Update()
        {
            // Diabolical. Why Pause Menu? Idk, maybe because it's shader has only requirements for that.
            Shader.SetGlobalFloat("UNSCALED_TIME", Time.unscaledTime);
        }
        public void QuitToMainMenu()
        {
            
            GameStateManager.LoadIntroScene();
        }
    }
}

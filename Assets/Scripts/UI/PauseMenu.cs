using UnityEngine;

namespace UI {
    public class PauseMenu : MonoBehaviour {
        private static PauseMenu _instance = null;

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
            _instance?.gameObject.SetActive(shown);
        }
    }
}

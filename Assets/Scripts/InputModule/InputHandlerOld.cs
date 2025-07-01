using UnityEngine;
using UnityEngine.Events;

namespace InputModule {
    public class InputHandlerOld : MonoBehaviour {
        private static InputHandlerOld _instance;

        private const int PrimaryButton = 0;

        private void Awake() {
            DontDestroyOnLoad(this);

            if (!_instance) {
                _instance = this;
            }
            else if (_instance != this) {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.N)) GotNKeyDown?.Invoke();
        }
        [System.Obsolete("Remove this shit")]
        public static event UnityAction GotPrimaryMouseButtonDown;
        [System.Obsolete("Remove this shit")]
        public static event UnityAction GotPrimaryMouseButtonUp;
        [System.Obsolete("Remove this shit")]
        public static event UnityAction GotEscapeKeyDown;
        
        public static event UnityAction GotNKeyDown;
    }
}

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

        private void Update() {
            if (Input.GetMouseButtonDown(PrimaryButton)) GotPrimaryMouseButtonDown?.Invoke();

            if (Input.GetMouseButtonUp(PrimaryButton)) GotPrimaryMouseButtonUp?.Invoke();

            if (Input.GetKeyDown(KeyCode.Space)) GotEscapeKeyDown?.Invoke();
            if (Input.GetKeyDown(KeyCode.Escape)) GotEscapeKeyDown?.Invoke();

            if (Input.GetKeyDown(KeyCode.N)) GotNKeyDown?.Invoke();
        }

        public static event UnityAction GotPrimaryMouseButtonDown;
        public static event UnityAction GotPrimaryMouseButtonUp;

        public static event UnityAction GotEscapeKeyDown;
        
        public static event UnityAction GotNKeyDown;
    }
}

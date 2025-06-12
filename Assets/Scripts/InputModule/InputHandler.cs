using UnityEngine;
using UnityEngine.Events;

namespace InputModule {
    namespace InputModule {
        public class InputHandler : MonoBehaviour {
            private const int PrimaryButton = 0;

            private void Update() {
                if (Input.GetMouseButtonDown(PrimaryButton)) GotPrimaryMouseButtonDown?.Invoke();

                if (Input.GetMouseButtonUp(PrimaryButton)) GotPrimaryMouseButtonUp?.Invoke();

                if (Input.GetKeyDown(KeyCode.Escape)) GotEscapeKeyDown?.Invoke();
            }

            public static event UnityAction GotPrimaryMouseButtonDown;
            public static event UnityAction GotPrimaryMouseButtonUp;

            public static event UnityAction GotEscapeKeyDown;
        }
    }
}

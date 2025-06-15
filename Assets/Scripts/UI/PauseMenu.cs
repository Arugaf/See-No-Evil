using System.Collections;
using UnityEngine;

namespace UI {
    public class PauseMenu : MonoBehaviour {
        private static PauseMenu _instance = null;
        [SerializeField] private Animator anim;
        [SerializeField] private float hidTime = 0.25f;
        [SerializeField] GameObject MenuObject;
        private Coroutine hidTimeCoroutine;
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
            _instance?.ChangeState(shown);
        }
        private IEnumerator HideMainMene()
        {
            yield return new WaitForSecondsRealtime(hidTime);
            MenuObject.SetActive(false);
        }
        private void ChangeState(bool shown)
        {
            if (hidTimeCoroutine != null) StopCoroutine(hidTimeCoroutine);
            if (shown)
            {
                MenuObject.SetActive(true);
            }
            else
            {
                hidTimeCoroutine = StartCoroutine(HideMainMene());
            }
            anim.SetBool("Show", shown);
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

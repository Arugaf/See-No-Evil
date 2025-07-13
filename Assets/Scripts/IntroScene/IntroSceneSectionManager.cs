using System.Collections;
using Cysharp.Threading.Tasks;
using Features.IntroScene;
using SaveManager;
using UnityEngine;
namespace Features.IntroScene {
    public class IntroSceneSectionManager : MonoBehaviour
    {
        [SerializeField] private ListDictionaryContainer<AbstractIntroSceneStage> introSceneStages;
        [SerializeField] private string InitID = "main";
        private string prevIndex;
        private bool transition = false;
        public IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            prevIndex = InitID;
            // All IntroScenes are deactivated by default;
            if (introSceneStages.TryGetValue(InitID, out var result))
                yield return result.SetActivation(true).ToCoroutine();
        }
        public void ChangeSection(string newID)
        {
            if (!transition)
            {
                if (!introSceneStages.TryGetValue(newID, out _))
                {
                    Debug.LogError($"There is no Stage with ID \"{newID}\"");
                    return;
                }
                transition = true;
                ChangeSectionTask(newID).Forget();
            }
        }
        private async UniTask ChangeSectionTask(string newID)
        {
            UniTask a = introSceneStages[prevIndex].SetActivation(false);
            UniTask b = introSceneStages[newID].SetActivation(true);
            prevIndex = newID;
            await UniTask.WhenAll(a, b);
            transition = false;
        }
    }
}
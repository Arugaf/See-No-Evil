using Cysharp.Threading.Tasks;
using Features.IntroScene;
using UnityEngine;

public class ApplicationQuitIntroSceneSection : AbstractIntroSceneStage
{
    [SerializeField] private GameObject couldDoApplicationQuit;
    [SerializeField] private float transitionTime = 2.0f;
    public void Awake()
    {
        #if UNITY_WEBGL
        couldDoApplicationQuit.SetActive(false);
        #endif
    }
    public override async UniTask SetActivation(bool active)
    {
        if (active)
        {
            await UniTask.WaitForSeconds(transitionTime);
            Application.Quit();
        }
    }
}

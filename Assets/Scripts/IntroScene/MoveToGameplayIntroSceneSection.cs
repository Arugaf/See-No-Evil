using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Features.IntroScene;
using Unity.Cinemachine;
using UnityEngine;
using VContainer;

public class MoveToGameplayIntroSceneSection : AbstractIntroSceneStage
{
    [SerializeField] private CinemachineCamera virtualCamera;
    [SerializeField] private IntroSceneDarknessRegulator regulator;
    [SerializeField] private float transitionTime = 2.0f;
    [SerializeField] private AudioSource tranisitonSource;
    private GameStateManager manager;
    [Inject]
    private void Construct(GameStateManager manager)
    {
        this.manager = manager;
    }
    public override async UniTask SetActivation(bool active)
    {
        if (active)
        {
            virtualCamera.enabled = active;
            tranisitonSource.Play();
            regulator.SetDarknessFactor(1.0f);
            await UniTask.WaitForSeconds(transitionTime);
            manager.LoadGame();
        }
    }
}

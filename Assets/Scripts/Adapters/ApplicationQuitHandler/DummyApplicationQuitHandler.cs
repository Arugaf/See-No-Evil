using UnityEngine;
using VContainer;
using VContainer.Unity;

public class DummyApplicationQuitHandler: MonoBehaviour, IInitializable
{
    private IApplicationQuitAction action;
    public void Initialize()
    {
        
    }

    [Inject]
    private void Construct(IApplicationQuitAction action)
    {
        this.action = action;
    }
    public void OnApplicationQuit()
    {
        action.OnApplicationQuit();
    }
}
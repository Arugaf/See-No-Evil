using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;
public class PluginYGApplicationQuitHandler : MonoBehaviour, IInitializable
{
    private bool called = false;
    private IApplicationQuitAction action;
    [Inject]
    private void Construct(IApplicationQuitAction action)
    {
        this.action = action;
    }
    public void QuitMessage()
    {
        Debug.Log("PLUGIN YG IS WORKING");
        Perform();
    }
    public void OnApplicationQuit()
    {
        Debug.Log("CLASSICAL QUIT WORKING");
        Perform();
    }
    private void Perform()
    {
        if (called) return;
        called = true;
        action.OnApplicationQuit();
    }
    // why do i even bother
    public void Initialize()
    {
        
    }
}

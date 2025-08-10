using Cysharp.Threading.Tasks;
using External;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public abstract class GenericLinkButton<T> : MonoBehaviour where T : ITemporaryAvailable
{
    protected T service;
    private bool locked = false;
    protected virtual bool TryHidingOnDone => true;
    [Inject]
    private void Construct(T value)
    {
        service = value;
    }
    public void OnEnable()
    {
        gameObject.SetActive(service.IsAvailable);
    }
    public void OnButtonAction()
    {
        if (locked) return;
        locked = true;
        Action().ContinueWith(ButtonDone).AttachExternalCancellation(destroyCancellationToken).Forget();
    }
    private void ButtonDone()
    {
        locked = false;
        if (TryHidingOnDone) OnEnable();
        else GetComponent<Button>().interactable = service.IsAvailable;
    }
    protected abstract UniTask Action();
}

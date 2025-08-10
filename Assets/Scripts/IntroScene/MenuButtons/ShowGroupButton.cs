using Cysharp.Threading.Tasks;
using External;
using System;
using VContainer;

public class ShowGroupButton: GenericLinkButton<ILinkOpener>
{
    [Serializable]
    public class Settings
    {
        public string GroupURL;
    }
    private Settings settings;
    [Inject]
    private void Construct(Settings s)
    {
        settings = s;
    }
    protected override UniTask Action()
    {
        return service.OpenLink(settings.GroupURL);
    }
}

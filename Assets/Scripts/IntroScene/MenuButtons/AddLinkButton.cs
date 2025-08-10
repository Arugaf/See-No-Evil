using Cysharp.Threading.Tasks;
using External;
using System.Threading.Tasks;
public class AddLinkButton : GenericLinkButton<IAddAsLinkButton>
{
    protected override bool TryHidingOnDone => false;
    protected override async UniTask Action()
    {
        await service.AddAsLink();
    }
}

using Cysharp.Threading.Tasks;
namespace External
{
    public class DisabledAddAsLinkButton: IAddAsLinkButton
    {
        public bool IsAvailable => false;
        public UniTask AddAsLink()
        {
            return UniTask.CompletedTask;
        }
    }
}

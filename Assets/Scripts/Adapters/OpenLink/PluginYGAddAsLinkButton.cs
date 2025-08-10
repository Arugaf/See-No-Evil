using Cysharp.Threading.Tasks;
using YG;
namespace External
{
    public class PluginYGAddAsLinkButton : IAddAsLinkButton
    {
        public bool IsAvailable => YG2.gameLabelCanShow;

        public UniTask AddAsLink()
        {
            YG2.GameLabelShowDialog();
            return UniTask.CompletedTask;
        }
    }
}

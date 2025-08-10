using Cysharp.Threading.Tasks;
using YG;
namespace External
{
    public class PluginYGLinkOpener: ILinkOpener
    {
        public bool IsAvailable => true;
        public UniTask OpenLink(string link)
        {
            YG2.OnURL(link);
            return UniTask.CompletedTask;
        }
    }
}

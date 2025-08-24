using UnityEngine;
using Cysharp.Threading.Tasks;
namespace External
{
    public class BasicLinkOpener : ILinkOpener
    {
        public bool IsAvailable => true;

        public UniTask OpenLink(string link)
        {
            Application.OpenURL(link);
            return UniTask.CompletedTask;
        }
    }
}

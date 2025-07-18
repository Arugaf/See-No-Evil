using Cysharp.Threading.Tasks;
namespace Tutorial
{
    public interface ITutorialSection
    {
        public UniTask Perform(ITutorialView view);
    }
}
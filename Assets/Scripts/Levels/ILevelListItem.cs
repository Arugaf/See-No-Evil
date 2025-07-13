using Cysharp.Threading.Tasks;

namespace Levels
{
    public interface ILevelListItem {
        public GameLevelInfoObject LevelInfoObject { get; }
        public bool IsUnlocked { get; }
        public bool IsSelectedAsMain{ get; }
        public UniTask<string> GetStatDescription();
    }
}
using Cysharp.Threading.Tasks;
namespace Levels
{
    public abstract class DullLevelListItem : ILevelListItem
    {
        public GameLevelInfoObject LevelInfoObject{ get; private set; }
        public bool IsUnlocked { get; private set; }
        public bool IsSelectedAsMain{ get; private set; }
        public DullLevelListItem(GameLevelInfoObject obj, bool isUnlocked, bool isSelected)
        {
            LevelInfoObject = obj;
            IsUnlocked = isUnlocked;
            IsSelectedAsMain = isSelected;
        }

        public abstract UniTask<string> GetStatDescription();
    }
}
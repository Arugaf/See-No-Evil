using System.Collections.Generic;
namespace Levels
{
    public interface IGameLevelManager
    {
        public IEnumerable<ILevelListItem> GetLevelInfo();
        public void SetLevel(ILevelListItem levelListItem);
    }
}
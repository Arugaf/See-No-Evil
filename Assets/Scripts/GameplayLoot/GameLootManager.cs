using SaveManager;
namespace Gameplay.Loot
{
    public class GameLootManager
    {
        ILootGameSaveManager saveManager;
        
        public GameLootManager(ILootGameSaveManager saveManager)
        {
            this.saveManager = saveManager;
        }
        public void Append()
        {
            
        }
    }
}

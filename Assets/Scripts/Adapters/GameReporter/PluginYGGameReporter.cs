using YG;
namespace External
{
    public class PluginYGGameReporter : IGameReporter
    {
        public void GameStarted()
        {
            YG2.GameReadyAPI();
        }
    }
}
namespace Leaderboard
{
    public class DummyLeaderboardManager : ILeaderboardManager
    {
        public bool IsAvailable => false;

        public ILeaderboard GetLeaderboard(string key)
        {
            return null;
        }
    }
}
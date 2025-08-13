using YG;
namespace External
{
    public class PluginYGGameReporter : IGameReporter
    {
        private bool _playing = false;
        private bool gameReadyInited = false;
        public bool IsPlaying { get => _playing; set  {
                if (_playing != value) PlayingUpdated(value);
            }
        }
        
        public void GameIsReadyAndInteractable()
        {
            if (!gameReadyInited)
            {
                YG2.GameReadyAPI();
                gameReadyInited = true;
            }
        }
        private void PlayingUpdated(bool value)
        {
            _playing = value;
            if (_playing)
            {
                YG2.GameplayStart();
            } else
            {
                YG2.GameplayStop();
            }
        }
    }
}
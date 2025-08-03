using System;
using UnityEngine.Audio;
using VContainer;
using VContainer.Unity;
namespace Features.VFX
{
    public class DarknessAudioManager : ITickable
    {
        [Serializable]
        public struct Settings
        {
            public float transitionTime;
            public AudioMixerSnapshot darknessSnapshot;
            public AudioMixerSnapshot normalSnapshot;
        }
        private GameplayDarknessManager manager;
        private Settings settings;
        private bool wasEnabled;
        [Inject]
        public DarknessAudioManager(GameplayDarknessManager manager, Settings settings)
        {
            this.manager = manager;
            this.settings = settings;
        }
        public void Tick()
        {
            if (manager.EnableDarkness != wasEnabled)
            {
                if (manager.EnableDarkness)
                {
                    settings.darknessSnapshot.TransitionTo(settings.transitionTime);
                }
                else
                {
                    settings.normalSnapshot.TransitionTo(settings.transitionTime);
                }
            }
            wasEnabled = manager.EnableDarkness;
        }

        
    }
}

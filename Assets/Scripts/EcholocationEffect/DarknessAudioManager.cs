using System;
using UnityEngine;
using UnityEngine.Audio;
using VContainer;
using VContainer.Unity;
namespace Features.VFX
{
    public class DarknessAudioManager : ITickable, IStartable
    {
        [Serializable]
        public struct Settings
        {
            public float transitionTime;
            public AudioMixerSnapshot darknessSnapshot;
            public AudioMixerSnapshot normalSnapshot;
            public AudioMixerGroup sfxMixerGroup;
            public float echolocationSoundVolume;
            public AudioClip echolocationUseSound;
        }
        private GameplayDarknessManager manager;
        private Settings settings;
        private bool wasEnabled;
        private AudioSource darknessAudio;
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
                    darknessAudio?.Play();
                    settings.darknessSnapshot.TransitionTo(settings.transitionTime);
                }
                else
                {
                    settings.normalSnapshot.TransitionTo(settings.transitionTime);
                }
            }
            wasEnabled = manager.EnableDarkness;
        }

        public void Start()
        {
            GameObject gm = new GameObject();
            var aud = gm.AddComponent<AudioSource>();
            aud.playOnAwake = false;
            aud.outputAudioMixerGroup = settings.sfxMixerGroup;
            aud.clip = settings.echolocationUseSound;
            aud.volume = settings.echolocationSoundVolume;
            darknessAudio = aud;
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.AudioManager
{
    [Serializable]
    public class AudioPlayRandomParameters: IAudioPlayParamGenerator
    {
        public Vector2 pitchRange;
        public Vector2 volumeRange;
        public Vector2 distanceRange;
        public Gradient EchoAnnotation;
        public List<AudioClip> Clips;
        public AudioPlayDeterminedParams Generate()
        {
            float pitch = UnityEngine.Random.Range(pitchRange.x, pitchRange.y);
            float volume = UnityEngine.Random.Range(pitchRange.x, pitchRange.y);
            float distance = UnityEngine.Random.Range(distanceRange.x, distanceRange.y);
            AudioClip clip = Clips[UnityEngine.Random.Range(0, Clips.Count)];
            return new AudioPlayDeterminedParams(pitch, distance, volume, EchoAnnotation, clip);
        }
    }
}

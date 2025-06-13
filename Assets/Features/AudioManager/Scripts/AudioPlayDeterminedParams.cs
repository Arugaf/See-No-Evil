using System;
using UnityEngine;

namespace Features.AudioManager
{
    public class AudioPlayDeterminedParams : IAudioPlayParamGenerator
    {
        public float Pitch;
        public float Distance;
        public float VolumeMultiplier;
        public Gradient EchoAnnotation;
        public AudioClip Clip;
        public float SoundDuration => (Clip.length / Math.Abs(Pitch));
        public AudioPlayDeterminedParams(float pitch, float distance, float volume, Gradient echoAnnot, AudioClip clip)
        {
            Pitch = pitch;
            Distance = distance;
            VolumeMultiplier = volume;
            EchoAnnotation = echoAnnot;
            Clip = clip;
        }

        public AudioPlayDeterminedParams Generate() => this;
    }
}

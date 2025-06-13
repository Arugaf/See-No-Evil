using UnityEngine;

namespace Features.AudioManager
{
    public class EcholocationAudioEffect : MonoBehaviour, IAudioEffect
    {
        
        [SerializeField] private AudioSource source;
        [SerializeField] private ParticleSystem echolocationParticles;
        [SerializeField] private float audioSourceDistanceMultiplicator = 5;

        public void Delete() => Destroy(gameObject);

        public void Play(in AudioPlayDeterminedParams playParams)
        {
            ParticleSystem.MainModule mod = echolocationParticles.main;
            var size = mod.startSize;
            size.mode = ParticleSystemCurveMode.Constant;
            size.constant = playParams.Distance;
            mod.startSize = size;

            var lifetime = mod.startLifetime;
            lifetime.mode = ParticleSystemCurveMode.Constant;
            lifetime.constant = playParams.SoundDuration;
            mod.startLifetime = lifetime;

            var cr = echolocationParticles.colorOverLifetime;
            var colorOverLifetime = cr.color;
            colorOverLifetime.mode = ParticleSystemGradientMode.Gradient;
            colorOverLifetime.gradient = playParams.EchoAnnotation;
            cr.color = colorOverLifetime;

            source.maxDistance = playParams.Distance * audioSourceDistanceMultiplicator;
            source.pitch = playParams.Pitch;
            source.PlayOneShot(playParams.Clip, playParams.VolumeMultiplier);
            echolocationParticles.Play();
        }

        public void SetActive(bool active) => gameObject.SetActive(active);

        public void SetPosition(Vector3 position) => transform.position = position;
    }
}

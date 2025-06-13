using UnityEngine;
using UnityEngine.Events;
namespace Features.AudioManager
{
    //public class AudioPheripheralSettings: ScriptableObject
    //{
    //    [SerializeField] private float SoundSpeed;
    //    [Range(0, 1)] public float SoundExpDecayPerMeter;
    //    public float GetSoundDecay(Vector3 origin, Vector3 me)
    //    {
    //        Vector3 distance = origin - me;
    //        return Mathf.Pow(1 - SoundExpDecayPerMeter, distance.magnitude);
    //    }
    //    public float GetSoundTime(Vector3 origin, Vector3 me)
    //    {
    //        Vector3 distance = origin - me;
    //        return distance.magnitude / SoundSpeed;
    //    }
    //}
    public class AudioTrigger: MonoBehaviour, IAudioCallbackReciever
    {
        public UnityEvent OnAudioTriggered;
        
        [SerializeField] public float maxSensivity;
        public void Start()
        {
            destroyCancellationToken.Register(AudioManager.RegisterAudioCallbackReciever(this));
        }
        public void AudioPlays(AudioPlayDeterminedParams param, Vector3 position)
        {
            Vector3 dist = transform.position - position;
            if (dist.magnitude > param.Distance) return;
            float sensivity = dist.magnitude / param.Distance;
            if (sensivity < maxSensivity) return;
            Invoke(nameof(HeardSomething), param.SoundDuration * sensivity);
        }
        void HeardSomething() => OnAudioTriggered?.Invoke();
    }
}
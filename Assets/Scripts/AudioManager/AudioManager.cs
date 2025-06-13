using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Features.AudioManager
{
    public enum SoundType { Normal = 0, Loud = 1 };
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager Instance;
        private Dictionary<SoundType, IObjectPool<IAudioEffect>> effects = new Dictionary<SoundType, IObjectPool<IAudioEffect>>();
        private List<IAudioCallbackReciever> callbackRecievers = new List<IAudioCallbackReciever>(); 
        [SerializeField] private int EachPoolSize = 10;
        [Header("0 = Normal\n1 = Loud")]
        [SerializeField] private GameObject[] SoundPrefabs;
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else
            {
                Debug.LogWarning("Multiple Audio Managers :L");
                Destroy(gameObject);
                return;
            }
            for(int i = 0; i < 2; i++)
            {
                effects.Add((SoundType)i, new AudioEffectPool(SoundPrefabs[i], transform, EachPoolSize));
            }
        }

        public static void PlayAtomic(Vector3 position, AudioPlayDeterminedParams parameters)
        {
            if(Instance == null)
            {
                Debug.LogError("Atomic sounds not working because we should have a shitty singleton");
                return;
            }
            if (!Instance.effects.TryGetValue(parameters.SoundType, out var pool))
            {
                Debug.LogError("Setup atomic sound prefabs in SoundPrefabs");
                return;
            }
            pool.Get(out IAudioEffect effect);
            effect.SetPosition(position);
            effect.Play(parameters, out float lifetime);
            Instance.StartCoroutine(Instance.AtomicLifetime(pool, effect, lifetime));
            foreach(var callbacker in Instance.callbackRecievers)
            {
                callbacker.AudioPlays(parameters, position);
            }
        }
        private IEnumerator AtomicLifetime(IObjectPool<IAudioEffect> pool, IAudioEffect effect, float lifetime)
        {
            yield return new WaitForSeconds(lifetime);
            pool.Release(effect);
        }
        public static Action RegisterAudioCallbackReciever(IAudioCallbackReciever audioCallbackReciever)
        {
            if(Instance == null)
            {
                Debug.LogError("Could not listen to the sounds because we should have a shitty singleton");
                return null!;
            }
            Instance.callbackRecievers.Add(audioCallbackReciever);
            return () => Instance?.callbackRecievers.Remove(audioCallbackReciever);
        }
    }
}

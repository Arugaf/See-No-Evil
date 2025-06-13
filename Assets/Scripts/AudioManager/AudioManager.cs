using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Features.AudioManager
{
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager Instance;
        private IObjectPool<IAudioEffect> effects;
        private List<IAudioCallbackReciever> callbackRecievers = new List<IAudioCallbackReciever>(); 
        [SerializeField] private int PoolSize = 10;
        [SerializeField] private GameObject AtomicSoundPrefab;
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else
            {
                Debug.LogWarning("Multiple Audio Managers :L");
                Destroy(gameObject);
                return;
            }
            effects = new LinkedPool<IAudioEffect>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, PoolSize);
        }
        IAudioEffect CreatePooledItem()
        {
            var go = Instantiate(AtomicSoundPrefab, transform);
            go.name = "Atomic Sound";
            return go.GetComponent<IAudioEffect>();
        }
        void OnReturnedToPool(IAudioEffect system)
        {
            system.SetActive(false);
        }
        void OnTakeFromPool(IAudioEffect system)
        {
            system.SetActive(true);
        }

        void OnDestroyPoolObject(IAudioEffect system)
        {
            system.Delete();
        }

        public static void PlayAtomic(Vector3 position, AudioPlayDeterminedParams parameters)
        {
            if(Instance == null)
            {
                Debug.LogError("Atomic sounds not working because we should have a shitty singleton");
                return;
            }
            Instance.effects.Get(out IAudioEffect effect);
            effect.SetPosition(position);
            effect.Play(parameters);
            Instance.StartCoroutine(Instance.AtomicLifetime(effect, parameters.SoundDuration));
            foreach(var callbacker in Instance.callbackRecievers)
            {
                callbacker.AudioPlays(parameters, position);
            }
        }
        private IEnumerator AtomicLifetime(IAudioEffect effect, float lifetime)
        {
            yield return new WaitForSeconds(lifetime);
            effects.Release(effect);
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

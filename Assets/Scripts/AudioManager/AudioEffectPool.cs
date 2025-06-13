using UnityEngine;
using UnityEngine.Pool;

namespace Features.AudioManager
{
    public class AudioEffectPool : IObjectPool<IAudioEffect>
    {
        IObjectPool<IAudioEffect> effects;
        private GameObject prefab;
        private UnityEngine.Transform parent;

        public int CountInactive => effects.CountInactive;

        public AudioEffectPool(GameObject prefab, UnityEngine.Transform parent, int PoolSize = 100)
        {
            this.prefab = prefab;
            this.parent = parent;
            effects = new LinkedPool<IAudioEffect>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, PoolSize);
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
        IAudioEffect CreatePooledItem()
        {
            var go = GameObject.Instantiate(prefab, parent);
            go.name = $"{go.name}[PooledSound]";
            return go.GetComponent<IAudioEffect>();
        }

        public IAudioEffect Get()
        {
            return effects.Get();
        }

        public PooledObject<IAudioEffect> Get(out IAudioEffect v)
        {
            return effects.Get(out v);
        }

        public void Release(IAudioEffect element)
        {
            effects.Release(element);
        }

        public void Clear()
        {
            effects.Clear();
        }
    }
}

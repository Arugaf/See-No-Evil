using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
namespace Features.OutroScene
{
    public class GamblingItemView : MonoBehaviour
    {
        public class LoadedLoot : IDisposable
        {
            LootScriptableObject obj;
            AsyncOperationHandle<GameObject> addressHandle;
            private bool alreadyLoaded = false;
            public LoadedLoot(LootScriptableObject obj)
            {
                this.obj = obj;
            }

            public void Dispose()
            {
                if (alreadyLoaded)
                {
                    Addressables.Release(addressHandle);
                }
            }

            public async UniTask<GameObject> GetPrefab()
            {
                if (alreadyLoaded) return addressHandle.Result;
                addressHandle = Addressables.LoadAssetAsync<GameObject>(obj.ModelViewPrefab);
                await addressHandle;
                alreadyLoaded = true;
                return addressHandle.Result;
            }
            public GameObject LoadedPrefab => addressHandle.IsValid() ? addressHandle.Result : null;
        }
        [SerializeField] private Transform coreViewTransform;
        [SerializeField] private Quaternion basicRot;
        [SerializeField] private Animator anim;
        private LoadedLoot currentRef;
        private GameObject currentInstance;
        //private bool loading;
        private void Awake()
        {
            basicRot = coreViewTransform.transform.localRotation;
        }
        public static async UniTask<LoadedLoot> Preload(LootScriptableObject obj)
        {
            //await UniTask.WaitUntil(() => !loading);
            //loading = true;
            var result = new LoadedLoot(obj);
            await result.GetPrefab();
            return result;
            //loading = false;
        }
        public async UniTask ToShow(LootScriptableObject obj)
        {
            DisposeCurrentRef();
            await ToShow(await Preload(obj));
        }
        // The LoadedLoot is CONSUMED here, this class is now is responsible to dispose it.
        // This is a bad practice.
        public async UniTask ToShow(LoadedLoot obj)
        {
            if (obj.LoadedPrefab == null)
            {
                Debug.LogError("Please preload.");
                return;
            }
            DisposeCurrentRef();
            currentRef = obj;
            anim.gameObject.SetActive(true);
            anim.Play("init");
            await UniTask.WaitForEndOfFrame();
            currentInstance = GameObject.Instantiate(currentRef.LoadedPrefab, coreViewTransform);
            enabled = true;
        }
        void DisposeCurrentRef()
        {
            if (currentRef != null)
            {
                if (currentInstance != null) Destroy(currentInstance);
                currentRef.Dispose();
                currentRef = null;
                anim.gameObject.SetActive(false);
            }
        }
        private void OnEnable()
        {
            coreViewTransform.gameObject.SetActive(true);
            coreViewTransform.transform.localRotation = basicRot;
        }
        private void OnDisable()
        {
            anim.gameObject.SetActive(false);
            
        }
        public void Rotate(float degrees)
        {
            if(enabled) coreViewTransform.Rotate(new Vector3(0, degrees, 0));
        }
        void OnDestroy()
        {
            if (currentInstance != null)
            {
                Destroy(currentInstance);
            }
            if (currentRef != null)
            {
                currentRef.Dispose();
            }
        }
    }
}
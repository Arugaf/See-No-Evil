using Cysharp.Threading.Tasks;
using TMPro;
using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.AddressableAssets;
namespace Features.OutroScene
{
    public class GamblingItemView : MonoBehaviour
    {
        [SerializeField] private Transform coreViewTransform;
        [SerializeField] private Quaternion basicRot;
        [SerializeField] private Animator anim;
        private AssetReferenceGameObject currentRef;
        private GameObject currentInstance;
        private GameObject currentPrefab;
        //private bool loading;
        private void Awake()
        {
            basicRot = coreViewTransform.transform.localRotation;
        }
        public async UniTask Preload(LootScriptableObject obj)
        {
            //await UniTask.WaitUntil(() => !loading);
            //loading = true;
            if (currentRef != null)
            {
                OnDisable();
                GameObject.Destroy(currentInstance);
                currentPrefab = null;
                currentRef.ReleaseAsset();
            }
            currentRef = obj.ModelViewPrefab;
            currentPrefab = await currentRef.LoadAssetAsync();
            //loading = false;
        }
        public async UniTask ToShow(LootScriptableObject obj)
        {
            if (currentRef != obj.ModelViewPrefab || currentPrefab == null)
            {
                await Preload(obj);
            }
            currentInstance = GameObject.Instantiate(currentPrefab, coreViewTransform);
            enabled = true;
            anim.gameObject.SetActive(true);
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
            if (currentRef != null || currentInstance != null)
            {
                currentRef.ReleaseAsset();
            }
        }
    }
}
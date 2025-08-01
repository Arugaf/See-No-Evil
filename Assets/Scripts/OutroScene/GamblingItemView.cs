using Cysharp.Threading.Tasks;
using TMPro;
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
        private void Awake()
        {
            basicRot = coreViewTransform.transform.localRotation;
        }
        public async UniTask ToShow(LootScriptableObject obj)
        {
            if (currentRef != null)
            {
                enabled = false;
                GameObject.Destroy(currentInstance);
                currentRef.ReleaseAsset();
            }
            currentRef = obj.ModelViewPrefab;
            var prefab = await currentRef.LoadAssetAsync();
            currentInstance = GameObject.Instantiate(prefab, coreViewTransform);
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
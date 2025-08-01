using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Leaderboard;
using UnityEngine;
using VContainer;

namespace UI
{
    public class GlobalLeaderboardView : MonoBehaviour
    {
        private ILeaderboardManager manager;
        private ILeaderboard showing;
        [SerializeField] private GameObject buttonPrefab;
        [SerializeField] private Transform coreTransform;
        private List<GameObject> rows = new List<GameObject>();
        [Inject]
        private void Construct(ILeaderboardManager mnger)
        {
            manager = mnger;
        }
        void Awake()
        {
            if (!manager.IsAvailable) enabled = false;
            else
            {
                showing = manager.GetLeaderboard(ILeaderboardManager.LEADERBOARD_GLOBAL);
            }
        }
        void OnEnable()
        {
            if (showing != null) Setup().AttachExternalCancellation(destroyCancellationToken).Forget();
        }
        async UniTask Setup()
        {
            var data = await showing.FetchLeaderboard(3, 1);
            OnDisable();
            foreach (var dat in data)
            {
                var gm = Instantiate(buttonPrefab, coreTransform);
                gm.SetActive(true);
                gm.GetComponent<GlobalLeaderboardViewRow>().SetData(dat);
                rows.Add(gm);
            }
        }
        void OnDisable()
        {
            foreach (GameObject gm in rows) Destroy(gm);
            rows.Clear();
        }
    }
}
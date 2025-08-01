using Cysharp.Threading.Tasks;
using Leaderboard;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GlobalLeaderboardViewRow : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI displayName;
        [SerializeField] private TextMeshProUGUI rank;
        [SerializeField] private TextMeshProUGUI score;
        [SerializeField] private RawImage rawImage; 
        public void SetData(ILeaderboardData leaderboardData)
        {
            displayName.text = leaderboardData.DisplayName;
            rank.text = leaderboardData.Place.ToString();
            score.text = leaderboardData.Score.ToString();
            SetImage(leaderboardData).AttachExternalCancellation(destroyCancellationToken).Forget();
        }
        async UniTask SetImage(ILeaderboardData data)
        {
            var image = await data.FetchProfileImage();
            if(image != null) rawImage.texture = image;
        }
    }
}
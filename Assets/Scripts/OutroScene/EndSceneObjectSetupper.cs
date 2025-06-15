using Gameplay;
using TMPro;
using UnityEngine;
using GameResult = Gameplay.GameplayState.Result;
public class EndSceneObjectSetupper : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI textTime;
    [SerializeField] private GameObject victory;
    [SerializeField] private GameObject loss;
    [SerializeField] private string victoryText;
    [SerializeField] private string overtimeText;
    [SerializeField] private string killedText;
    [SerializeField] private string victoryRemainingTimeText;
    private void Awake()
    {
        var state = Gameplay.GameplayState.LastGameState;
        var time = Gameplay.GameplayState.LastGameTime;
        if (state == GameResult.Victory)
        {
            victory.SetActive(true);
            descriptionText.text = victoryText;
            textTime.text = string.Format(victoryRemainingTimeText, GameplayState.GetTimeSpec(time));
        }
        else if(state == GameResult.Killed)
        {
            loss.SetActive(true);
            descriptionText.text = killedText;
            textTime.text = "";
        }
        else
        {
            loss.SetActive(true);
            descriptionText.text = overtimeText;
            textTime.text = "";
        }
    }
}

using TMPro;
using UnityEngine;
using GameResult = Gameplay.GameplayState.Result;
public class EndSceneObjectSetupper : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private GameObject victory;
    [SerializeField] private GameObject loss;
    [SerializeField] private string victoryText;
    [SerializeField] private string overtimeText;
    [SerializeField] private string killedText;
    private void Awake()
    {
        var state = Gameplay.GameplayState.LastGameState;
        if (state == GameResult.Victory)
        {
            victory.SetActive(true);
            descriptionText.text = victoryText;
        }
        else if(state == GameResult.Killed)
        {
            loss.SetActive(true);
            descriptionText.text = killedText;
        }
        else
        {
            loss.SetActive(true);
            descriptionText.text = overtimeText;
        }
    }
}

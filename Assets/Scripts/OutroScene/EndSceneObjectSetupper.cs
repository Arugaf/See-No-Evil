using Cysharp.Threading.Tasks;
using Gameplay;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using GameResult = Gameplay.GameplayState.Result;
public class EndSceneObjectSetupper : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI textTime;
    [SerializeField] private GameObject victory;
    [SerializeField] private GameObject loss;
    [SerializeField] private LocalizedString victoryText;
    [SerializeField] private LocalizedString overtimeText;
    [SerializeField] private LocalizedString killedText;
    [SerializeField] private LocalizedString victoryRemainingTimeText;
    private async void Start()
    {
        var state = Gameplay.GameplayState.LastGameState;
        var time = Gameplay.GameplayState.LastGameTime;
        if (state == GameResult.Victory)
        {
            victory.SetActive(true);
            descriptionText.text = await victoryText.GetLocalizedStringAsync();
            var arguments = new Dictionary<string, string> { { "Time", GameplayState.GetTimeSpec(time) } };
            textTime.text = await victoryRemainingTimeText.GetLocalizedStringAsync(arguments);
        }
        else if (state == GameResult.Killed)
        {
            loss.SetActive(true);
            descriptionText.text = await killedText.GetLocalizedStringAsync();
            textTime.text = "";
        }
        else
        {
            loss.SetActive(true);
            descriptionText.text = await overtimeText.GetLocalizedStringAsync();
            textTime.text = "";
        }
    }
}

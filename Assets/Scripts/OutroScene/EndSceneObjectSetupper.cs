using Cysharp.Threading.Tasks;
using Gameplay;
using Gameplay.LevelStats;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using VContainer;
using GameResult = Gameplay.GameplayResultStorage.Result;
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
    private IScoreEvaluator scoreEvaluator;
    private ILevelStatsManager levelStatsManager;
    private GameplayResultStorage gameplayResultStorage;
    [Inject]
    private void Construct(GameplayResultStorage resultStorage, IScoreEvaluator scoreEvaluator, ILevelStatsManager levelStatsManager)
    {
        gameplayResultStorage = resultStorage;
        this.scoreEvaluator = scoreEvaluator;
        this.levelStatsManager = levelStatsManager;
    }
    private async void Start()
    {
        var state = gameplayResultStorage.LastGameState;
        var time = gameplayResultStorage.LastGameTime;
        if (state == GameResult.Victory)
        {
            victory.SetActive(true);
            int totalScore = scoreEvaluator.Evaluate(gameplayResultStorage);
            descriptionText.text = await victoryText.GetLocalizedStringAsync();
            levelStatsManager.SubmitResult(totalScore, gameplayResultStorage);
            var arguments = new Dictionary<string, string> { { "Time", GameplayResultStorage.GetTimeSpec(time) },
                                                             { "Score", totalScore.ToString()} };
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

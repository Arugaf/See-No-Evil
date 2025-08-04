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
namespace Features.OutroScene
{
    public class EndSceneObjectSetupper : EndSceneManagerBehaviour
    {
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private TextMeshProUGUI textTime;
        [SerializeField] private GameObject victory;
        [SerializeField] private GameObject loss;
        [SerializeField] private GameObject nextLevelButton;
        [SerializeField] private LocalizedString victoryText;
        [SerializeField] private LocalizedString overtimeText;
        [SerializeField] private LocalizedString killedText;
        [SerializeField] private LocalizedString victoryRemainingTimeText;
        private IScoreEvaluator scoreEvaluator;
        private ILevelStatsManager levelStatsManager;
        private ILevelDefinition levelDefinition;
        private GameplayResultStorage gameplayResultStorage;
        private GameLevelInfoObject nextLevel;
        [Inject]
        private void Construct(GameplayResultStorage resultStorage, IScoreEvaluator scoreEvaluator, ILevelStatsManager levelStatsManager, ILevelDefinition levelDefinition)
        {
            gameplayResultStorage = resultStorage;
            this.scoreEvaluator = scoreEvaluator;
            this.levelStatsManager = levelStatsManager;
            this.levelDefinition = levelDefinition;
        }
        public async override UniTask Init()
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
        private void SetupButton()
        {
            if (gameplayResultStorage.LastGameState == GameResult.Victory)
            {
                if (levelDefinition.TryGetNext(gameplayResultStorage.gameLevelInfo.ID, out nextLevel))
                {
                    nextLevelButton.SetActive(true);
                }
                else
                {
                    nextLevelButton.SetActive(false);
                }
            }
            else
            {
                nextLevelButton.SetActive(false);
            }
        }
    }
}
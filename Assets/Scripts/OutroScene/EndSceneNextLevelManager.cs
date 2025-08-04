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
    public class EndSceneNextLevelManager : EndSceneManagerBehaviour
    {
        [SerializeField] private GameObject buttonToNextLevel;
        [SerializeField] private EndSceneManager endSceneManager;
        private ILevelDefinition levelDefinition;
        private GameplayResultStorage gameplayResultStorage;
        private GameLevelInfoObject levelInfoObject;
        [Inject]
        private void Construct(GameplayResultStorage resultStorage, ILevelDefinition levelDefinition)
        {
            gameplayResultStorage = resultStorage;
            this.levelDefinition = levelDefinition;
        }
        public override UniTask Init()
        {
            var state = gameplayResultStorage.LastGameState;
            var time = gameplayResultStorage.LastGameTime;
            if (state == GameResult.Victory)
            {
                if (levelDefinition.TryGetNext(gameplayResultStorage.gameLevelInfo.ID, out levelInfoObject))
                {
                    buttonToNextLevel.SetActive(true);
                }
                else
                {
                    buttonToNextLevel.SetActive(false);
                }
            }
            else
            {
                buttonToNextLevel.SetActive(false);
            }
            return UniTask.CompletedTask;
        }
        public void OnNextLevelButton()
        {
            if (endSceneManager.EnsureTransition(true))
            {
                gameplayResultStorage.SetLevel(levelInfoObject);
            }
        }
    }
}
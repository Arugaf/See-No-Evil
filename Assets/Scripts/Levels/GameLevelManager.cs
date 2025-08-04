using System;
using System.Collections.Generic;
using Gameplay;
using Gameplay.LevelStats;
using UnityEngine.Localization;
using VContainer;
namespace Levels
{
    public class GameLevelManager : IGameLevelManager
    {
        [Serializable]
        public class Settings
        {
            public LocalizedString fullStatString;
            public LocalizedString emptyStatString;
            public LocalizedString lockedLevelString;
        }
        private ILevelDefinition levelDefinition;
        private ILevelStatsManager statsManager;
        private GameplayResultStorage resultStorage;
        private Settings settings;
        [Inject]
        public GameLevelManager(ILevelDefinition definition, ILevelStatsManager levelStatsManager, Settings settings, GameplayResultStorage resultStorage)
        {
            levelDefinition = definition;
            statsManager = levelStatsManager;
            this.settings = settings;
            this.resultStorage = resultStorage;
        }
        public IEnumerable<ILevelListItem> GetLevelInfo()
        {
            bool lastWasUnlocked = true;
            foreach (var level in levelDefinition.Levels)
            {
                bool isMain = level.ID == statsManager.LastPlayedLevelID;
                if (statsManager.TryGetData(level.ID, out var stats))
                {
                    yield return new PlayedLevelListItem(level, settings.fullStatString, stats.Time, stats.Score, isMain);
                    lastWasUnlocked = true;
                }
                else
                {
                    yield return new UndiscoveredLevelListItem(level, lastWasUnlocked ? settings.emptyStatString : settings.lockedLevelString, lastWasUnlocked, isMain);
                    lastWasUnlocked = false;
                }
            }
        }

        public void SetLevel(ILevelListItem levelListItem)
        {
            if (levelListItem.IsUnlocked) resultStorage.SetLevel(levelListItem.LevelInfoObject);
            statsManager.LastPlayedLevelID = levelListItem.LevelInfoObject.ID;
        }
        // public bool HasNextLevel()
        // {
            
        // }
    }
}
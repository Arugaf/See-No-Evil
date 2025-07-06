using System;
using UnityEngine;
using UnityEngine.Events;
using VContainer.Unity;
namespace Gameplay.Loot
{
    public abstract class GambleBoxBehaviour: MonoBehaviour
    {
        public event UnityAction OnBeingPickedUp;
    }
    public interface ILootSpawner
    {
        public GambleBoxBehaviour CreateLootAtRandomPos(GameObject prefab);
    }
    public class GameplayLootManager : IStartable, IDisposable
    {
        public bool GotTheLoot { get; private set; }
        public event UnityAction OnLootPickedUp;
        private Func<GambleBoxBehaviour> lootSpawner;
        private GameplayResultStorage gameplayResultStorage;

        public GameplayLootManager(Func<GambleBoxBehaviour> lootSpawner, GameplayResultStorage gameplayResultStorage)
        {
            this.lootSpawner = lootSpawner;
            this.gameplayResultStorage = gameplayResultStorage;
        }

        public void Dispose()
        {
            // maybe GotTheLoot should just be the mirror of this field.
            gameplayResultStorage.AquiredPrize = GotTheLoot;
        }

        public void Start()
        {
            lootSpawner().OnBeingPickedUp += GameplayLootManager_OnBeingPickedUp;
        }

        private void GameplayLootManager_OnBeingPickedUp()
        {
            GotTheLoot = true;
            OnLootPickedUp?.Invoke();
        }
    }
}

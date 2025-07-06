using UnityEngine;
namespace Gameplay.Loot
{
    public interface ILootSpawner
    {
        public GambleBoxBehaviour CreateLootAtRandomPos(GameObject prefab);
    }
}

using System;
using UnityEngine;
namespace Gameplay.Loot
{
    [CreateAssetMenu(fileName = "RandomLootObject", menuName = "Scriptable Objects/RandomLootObject")]
    public class GambleBoxLootObject : ScriptableObject, IRandomPickable<LootScriptableObject>
    {
        [field: SerializeField] public GameObject InGamePrefab { get; private set; }
        [field: SerializeField] public GameObject ViewPrefab { get; private set; }
        [Serializable]
        public class LootPair : ObjectChancePair<LootScriptableObject>
        {
        }
        [SerializeField] private LootPair[] _chancePairs;
        public LootScriptableObject Pick(IRandom rnd)
        {
            return rnd.PickRandom(_chancePairs, name);
        }
    }
}
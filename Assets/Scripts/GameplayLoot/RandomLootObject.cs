using System;
using UnityEngine;
namespace Gameplay.Loot
{
    [CreateAssetMenu(fileName = "RandomLootObject", menuName = "Scriptable Objects/RandomLootObject")]
    public class RandomLootObject : ScriptableObject, IRandomPickable<LootScriptableObject>
    {
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
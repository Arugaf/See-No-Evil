using UnityEngine;
using Registries;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;
[CreateAssetMenu(fileName = "LootScriptableObject", menuName = "Scriptable Objects/Loot/LootScriptableObject")]
public class LootScriptableObject : IdentifiableScriptableObject
{
    [field: SerializeField] public LocalizedString Name { get; private set; }
    [field: SerializeField] public LocalizedString Description { get; private set; }
    [field: SerializeField] public AssetReferenceGameObject ModelViewPrefab { get; private set; }
    [field: SerializeField] public Sprite Image { get; private set; }
    [field: SerializeField] public int ScoreToGrant { get; private set; }
}

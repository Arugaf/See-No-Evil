using Gameplay.Loot;
using UnityEngine;
using VContainer;

public class GambleBoxUIIndicator : MonoBehaviour
{
    private GameplayLootManager lootManager;
    [SerializeField] private GameObject setActiveObject;
    [Inject]
    private void Construct(GameplayLootManager manager)
    {
        lootManager = manager;
        lootManager.OnLootPickedUp += Manager_OnLootPickedUp;
    }

    private void Manager_OnLootPickedUp()
    {
        setActiveObject.SetActive(true);
    }
}

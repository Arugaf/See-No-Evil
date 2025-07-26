using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Features.IntroScene;
using Features.OutroScene;
using Gameplay.Loot;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using VContainer;
public interface ILootSectionButton
{
    public event UnityAction<LootAndCount> OnShow;
    public void SetLootAndCount(LootAndCount cnt);
    public void SetSelected(bool selected);
}
public class LootSectionSubcomponent : IntroSceneStageSubcomponent
{
    [SerializeField] private GamblingItemView itemView;
    [SerializeField] private LocalizedTextController nameTextController;
    [SerializeField] private LocalizedString unselectedThings;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Transform buttonParent;
    private IGameLootManager gameLootManager;
    private List<GameObject> buttons = new List<GameObject>();
    private bool blockChange = false;
    private ILootSectionButton lastLootSecButton;
    [Inject]
    private void Construct(IGameLootManager manager)
    {
        gameLootManager = manager;
    }
    public override void IntentionForActivation(bool state)
    {
        if (state)
        {
            Init();
        }
        else
        {
            itemView.enabled = false;
            blockChange = true;
        }
    }
    private void Init()
    {
        blockChange = false;
        nameTextController.SetText(unselectedThings).Forget();
        lastLootSecButton = null;
        foreach (GameObject obj in buttons) Destroy(obj);
        buttons.Clear();
        foreach (var obj in gameLootManager.GetAllPossibleLoot())
        {
            GameObject gm = Instantiate(buttonPrefab, buttonParent);
            gm.SetActive(true);
            buttons.Add(gm);
            var secButton = gm.GetComponent<ILootSectionButton>();
            secButton.OnShow += (x) => Show(x, secButton);
            secButton.SetLootAndCount(obj);
        }
    }
    public void Show(LootAndCount lootAndCount, ILootSectionButton sec)
    {
        if (lootAndCount.Count > 0 && !blockChange)
        {
            lastLootSecButton?.SetSelected(false);
            lastLootSecButton = sec;
            lastLootSecButton.SetSelected(true);
            ShowProcess(lootAndCount).Forget();
        }
    }
    private async UniTask ShowProcess(LootAndCount count)
    {
        blockChange = true;
        await itemView.ToShow(count.Loot);
        await nameTextController.SetText(count.Loot.Name);
        blockChange = false;
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Gameplay;
using Levels;
using UnityEngine;
using UnityEngine.Assertions.Must;
using VContainer;
public class LevelSelectorUIController : MonoBehaviour
{
    private IGameLevelManager levelManager;
    [SerializeField] private LevelSelectorBar levelSelectorBar;
    [SerializeField] private LevelStatView levelStatView;
    private List<ILevelListItem> levelListItems;
    private int currentSelected = 0;
    private bool levelSetProcess = false;
    [Inject]
    private void Construct(IGameLevelManager manager)
    {
        levelManager = manager;
    }
    public void OnEnable()
    {
        levelListItems = levelManager.GetLevelInfo().ToList();
        levelSelectorBar.OnSelectedIndex += SelectedIndex;
        levelSelectorBar.Initialize(levelListItems);
    }
    private void SelectedIndex(int idx)
    {
        if (levelSetProcess) return;
        levelSetProcess = true;
        SetLevel(idx).Forget();
    }
    private async UniTask SetLevel(int idx)
    {
        currentSelected = idx;
        levelSelectorBar.SetSelection(idx);
        await levelStatView.Show(levelListItems[idx]);
        levelManager.SetLevel(levelListItems[idx]);
        levelSetProcess = false;
    }
    public void DeltaIndex(int delta)
    {
        // we adding levelListItems.Count here in order to handle negative indexes (-1)
        SelectedIndex((currentSelected + delta + levelListItems.Count) % levelListItems.Count);
    }
    public void OnDisable()
    {
        levelSelectorBar.OnSelectedIndex -= SelectedIndex;
    }
}

using System;
using System.Collections.Generic;
using Levels;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelSelectorBar : MonoBehaviour
{
    [Serializable]
    public struct LevelSelectionButtonStyle
    {
        public Color backColor;
    }
    [SerializeField] private GameObject buttonObject;
    [SerializeField] private LevelSelectionButtonStyle lockedStyle;
    [SerializeField] private LevelSelectionButtonStyle selectedStyle;
    [SerializeField] private LevelSelectionButtonStyle unlockedStyle;
    [SerializeField] private Transform buttonParent;
    private List<Image> spawnedButtons = new List<Image>();
    private List<bool> isButtonUnlocked = new List<bool>();
    private int currentSelected = -1;
    public event UnityAction<int> OnSelectedIndex;
    public void Initialize(IReadOnlyCollection<ILevelListItem> listItems)
    {
        Clear();
        currentSelected = -1;
        int i = 0;
        int main = 0;
        foreach (var item in listItems)
        {
            SetupButtonForIndex(i, item.IsUnlocked ? unlockedStyle : lockedStyle);
            isButtonUnlocked.Add(item.IsUnlocked);
            if (item.IsSelectedAsMain)
                main = i;
            i++;
        }
        OnSelect(main);
    }
    private void Clear()
    {
        isButtonUnlocked.Clear();
        foreach (var obj in spawnedButtons) Destroy(obj.gameObject);
        spawnedButtons.Clear();
    }
    private void SetupButtonForIndex(int idx, in LevelSelectionButtonStyle style)
    {
        GameObject button = Instantiate(buttonObject, buttonParent);
        button.SetActive(true);
        button.GetComponent<Button>().onClick.AddListener(() => { OnSelect(idx); });
        var image = button.GetComponent<Image>();
        spawnedButtons.Add(image);
        SetStyle(idx, style); 
    }
    private void SetStyle(int idx, in LevelSelectionButtonStyle style)
    {
        spawnedButtons[idx].color = style.backColor;
    }
    private void OnSelect(int idx)
    {
        if (idx != currentSelected) OnSelectedIndex?.Invoke(idx);
    }
    public void SetSelection(int idx)
    {
        if(currentSelected >= 0) SetStyle(currentSelected, isButtonUnlocked[currentSelected] ? unlockedStyle : lockedStyle);
        currentSelected = idx;
        SetStyle(currentSelected, selectedStyle);
    }
}

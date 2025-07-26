using System;
using Gameplay.Loot;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LootSectionButton : MonoBehaviour, ILootSectionButton
{
    [Serializable]
    public struct Style
    {
        public Color mainColor;
        public Color backgroundColor;
    }
    public event UnityAction<LootAndCount> OnShow;
    [SerializeField] private Image mainImage;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Style selectedStyle;
    [SerializeField] private Style blockedStyle;
    [SerializeField] private Style obtainedStyle;
    private LootAndCount myLootAndCount;
    public void SetLootAndCount(LootAndCount cnt)
    {
        myLootAndCount = cnt;
        SetStyle(cnt.Count > 0 ? obtainedStyle : blockedStyle);
        mainImage.sprite = cnt.Loot.Image;
    }

    public void SetSelected(bool selected)
    {
        if (myLootAndCount.Count <= 0) return;
        SetStyle(selected ? selectedStyle : obtainedStyle);
    }
    private void SetStyle(Style style)
    {
        mainImage.color = style.mainColor;
        backgroundImage.color = style.backgroundColor;
    }
    public void OnPressed()
    {
        OnShow?.Invoke(myLootAndCount);
    }
}

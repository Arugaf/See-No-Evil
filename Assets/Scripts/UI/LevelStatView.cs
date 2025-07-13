using Cysharp.Threading.Tasks;
using Levels;
using TMPro;
using UnityEngine;

public class LevelStatView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelLabel;
    [SerializeField] private TextMeshProUGUI levelStats;
    [SerializeField] private GameObject enterText;
    public async UniTask Show(ILevelListItem levelListItem)
    {
        string levelD = await levelListItem.GetStatDescription();
        string name = await levelListItem.LevelInfoObject.LocalizedName.GetLocalizedStringAsync();
        levelStats.text = levelD;
        levelLabel.text = name;
        enterText.SetActive(levelListItem.IsUnlocked);
    }
}

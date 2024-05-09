using TMPro;
using UnityEngine;

public class QuestHUD : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _title;
    [SerializeField]
    private TextMeshProUGUI _description;

    private void Start()
    {
        EventManager.Instance.DisplayQuestChanged += UpdateQuestDisplay;
        EventManager.Instance.DisplayQuestHidden += HideDisplayQuest;
    }

    private void UpdateQuestDisplay(string title, string description)
    {
        _title.text = title;
        _description.text = description;
    }

    private void HideDisplayQuest()
    {
        // TODO
    }
}

using DG.Tweening;
using TMPro;
using UnityEngine;

public class QuestHUD : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _title;
    [SerializeField]
    private TextMeshProUGUI _description;

    private ManagedTween _tween = new();

    private void Start()
    {
        EventManager.Instance.DisplayQuestChanged += UpdateQuestDisplay;
        EventManager.Instance.DisplayQuestHidden += HideDisplayQuest;
    }

    private void UpdateQuestDisplay(string title, string description)
    {
        _tween.PlayNext(transform.DOLocalMoveX(-Mathf.Abs(transform.localPosition.x), 0.2f));
        _title.text = title;
        _description.text = description;
    }

    private void HideDisplayQuest()
    {
        _tween.PlayNext(transform.DOLocalMoveX(Mathf.Abs(transform.localPosition.x), 0.2f));
    }
}

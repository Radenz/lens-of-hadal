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

    private RectTransform _transform;

    private void Start()
    {
        _transform = (RectTransform)transform;

        EventManager.Instance.DisplayQuestChanged += UpdateQuestDisplay;
        EventManager.Instance.DisplayQuestHidden += HideDisplayQuest;
    }

    private void UpdateQuestDisplay(string title, string description)
    {
        _tween.PlayNext(transform.DOLocalMoveX(0, 0.2f));
        _title.text = title;
        _description.text = description;
    }

    private void HideDisplayQuest()
    {
        _tween.PlayNext(transform.DOLocalMoveX(_transform.rect.width, 0.2f));
    }
}

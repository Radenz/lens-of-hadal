using DG.Tweening;
using TMPro;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class QuestHUD : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _title;
    [SerializeField]
    private TextMeshProUGUI _description;

    private ManagedTween _tween = new();

    [SerializeField]
    private bool _isStaticDisplay = false;

    private RectTransform _transform;

    private void Start()
    {
        _transform = (RectTransform)transform;

        if (!_isStaticDisplay)
        {
            EventManager.Instance.DisplayQuestChanged += UpdateQuestDisplay;
            EventManager.Instance.DisplayQuestHidden += HideDisplayQuest;
        }
        else
        {
            QuestData currentQuest = QuestManager.Instance.CurrentQuest.Data;
            QuestStep currentStep = QuestManager.Instance.CurrentQuest.CurrentStep.GetComponent<QuestStep>();
            UpdateQuestDisplay(
                currentQuest.Title,
                currentStep.Description
                );
        }
    }

    private void UpdateQuestDisplay(string title, string description)
    {
        if (!_isStaticDisplay)
            _tween.PlayNext(transform.DOLocalMoveX(0, 0.2f));
        _title.text = title;
        _description.text = description;
    }

    private void HideDisplayQuest()
    {
        _tween.PlayNext(transform.DOLocalMoveX(_transform.rect.width, 0.2f));
    }
}

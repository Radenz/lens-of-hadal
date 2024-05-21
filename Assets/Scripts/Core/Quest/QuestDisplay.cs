using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class QuestDisplay : MonoBehaviour
{
    [HideInInspector]
    public QuestData Quest;

    [SerializeField]
    private TextMeshProUGUI _title;
    [SerializeField]
    private TextMeshProUGUI _description;

    [SerializeField]
    private GameObject _buttons;
    [SerializeField]
    private GameObject _acceptButton;
    [SerializeField]
    private GameObject _inProgressLabel;
    [SerializeField]
    private GameObject _claimRewardButton;
    [SerializeField]
    private GameObject _completedLabel;

    private void Start()
    {
        EventManager.Instance.QuestAccepted += OnAccept;
        EventManager.Instance.QuestCompleted += OnComplete;
        EventManager.Instance.QuestRewardClaimed += OnClaimReward;

        _acceptButton.SetActive(true);
        PopulateFields();
        _title.ForceMeshUpdate(true);
        _description.ForceMeshUpdate(true);
        RecomputeLayout();
        AddOffsetY(-Height());
    }

    private void OnAccept(QuestData quest)
    {
        if (quest != Quest)
        {
            _acceptButton.SetActive(false);
            return;
        }

        _acceptButton.SetActive(false);
        _inProgressLabel.SetActive(true);
    }

    private void OnComplete(QuestData quest)
    {
        if (quest != Quest) return;
        _inProgressLabel.SetActive(false);
        _claimRewardButton.SetActive(true);
    }

    private void OnClaimReward(QuestData quest)
    {
        if (quest != Quest)
        {
            if (Quest.IsCompleted())
                return;

            _acceptButton.SetActive(true);
            return;
        }

        quest.Reward.Give();
        _claimRewardButton.SetActive(false);
        _completedLabel.SetActive(true);
    }

    [Button]
    public void MakeAnonymous()
    {
        _title.text = "???";
        int lines = Quest.Description.Split("\n").Length;
        _description.text = "?????";
        for (int i = 0; i < lines - 1; i++)
        {
            _description.text += "\n?????";
        }
    }

    [Button]
    public void PopulateFields()
    {
        _title.text = Quest.Title;
        _description.text = Quest.Description;
    }

    public void Unlock()
    {
        PopulateFields();
        RecomputeLayout();
        QuestMaster.Instance.RecomputeLayout();
    }

    public void Accept()
    {
        EventManager.Instance.AcceptQuest(Quest);
    }

    public void ClaimReward()
    {
        EventManager.Instance.ClaimQuestReward(Quest);
    }

    public float Height()
    {
        RectTransform transform_ = (RectTransform)transform;
        return transform_.rect.height;
    }

    public void SetOffsetY(float offset)
    {
        RectTransform transform_ = (RectTransform)transform;
        Vector2 position = transform_.anchoredPosition;
        position.y = offset;
        transform_.anchoredPosition = position;
    }

    private void AddOffsetY(float diff)
    {
        RectTransform transform_ = (RectTransform)transform;
        Vector2 position = transform_.anchoredPosition;
        position.y += diff;
        transform_.anchoredPosition = position;
    }

    [Button]
    public void RecomputeLayout()
    {
        RectTransform transform_ = (RectTransform)transform;
        RectTransform titleTransform = (RectTransform)_title.transform;
        RectTransform descriptionTransform = (RectTransform)_description.transform;
        RectTransform buttonTransform = (RectTransform)_buttons.transform;

        _title.FitHeight();
        _description.FitHeight();

        Vector2 position = descriptionTransform.anchoredPosition;
        position.y = -titleTransform.rect.height;
        descriptionTransform.anchoredPosition = position;

        position = buttonTransform.anchoredPosition;
        position.y = -(titleTransform.rect.height + descriptionTransform.rect.height);
        buttonTransform.anchoredPosition = position;

        transform_.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
            titleTransform.rect.height + descriptionTransform.rect.height + buttonTransform.rect.height);
    }
}

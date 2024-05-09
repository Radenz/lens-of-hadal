using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class QuestDisplay : MonoBehaviour
{
    [HideInInspector]
    public Quest Quest;

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
        // TODO: impl
    }

    public void Complete()
    {
        // TODO: impl
    }

    public void ClaimReward()
    {
        // TODO: impl
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
        Debug.Log(_description.TotalHeight());
        Debug.Log(descriptionTransform.rect.height);

        position = buttonTransform.anchoredPosition;
        position.y = -(titleTransform.rect.height + descriptionTransform.rect.height);
        buttonTransform.anchoredPosition = position;

        transform_.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
            titleTransform.rect.height + descriptionTransform.rect.height + buttonTransform.rect.height);
    }
}

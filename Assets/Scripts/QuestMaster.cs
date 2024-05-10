using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class QuestMaster : Singleton<QuestMaster>
{
    private Transform _transform;

    [Header("Display Settings")]
    [SerializeField]
    private float _topMargin = 24;

    private float _totalHeight;

    [Header("Quest Settings")]
    [SerializeField]
    private List<QuestData> _quests;

    [SerializeField]
    private GameObject _questPrefab;

    protected override void Awake()
    {
        base.Awake();
        _transform = transform;
        _topMargin = _totalHeight;
    }

    private void OnEnable()
    {
        EventManager.Instance.QuestUnlocked += OnQuestUnlocked;
    }

    private void OnQuestUnlocked(QuestData quest)
    {
        if (_transform.childCount > 0)
        {
            RectTransform lastChild = (RectTransform)_transform.GetChild(_transform.childCount - 1);
            _totalHeight = lastChild.rect.height;
        }

        GameObject obj = Instantiate(_questPrefab, _transform);
        QuestDisplay display = obj.GetComponent<QuestDisplay>();
        display.Quest = quest;
        display.SetOffsetY(-_totalHeight);
    }

    [Button]
    private void Clear()
    {
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }

    [Button]
    private void Populate()
    {
        if (_quests == null) return;

        foreach (QuestData quest in _quests)
        {
            if (quest == null) return;

            GameObject obj = Instantiate(_questPrefab, transform);
            QuestDisplay display = obj.GetComponent<QuestDisplay>();
            display.Quest = quest;
            display.MakeAnonymous();
        }
    }

    [Button]
    public void RecomputeLayout()
    {
        float offset = _topMargin;

        foreach (Transform child in transform)
        {
            QuestDisplay display = child.GetComponent<QuestDisplay>();
            display.RecomputeLayout();
            offset += display.Height();
            display.SetOffsetY(-offset);
        }

        RectTransform transform_ = (RectTransform)transform;
        transform_.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, offset + _topMargin);
    }
}

using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[ExecuteAlways]
public class QuestMaster : Singleton<QuestMaster>
{
    [Header("Display Settings")]
    [SerializeField]
    private float _topMargin = 24;

    [Header("Quest Settings")]
    [SerializeField]
    private List<QuestData> _quests;

    [SerializeField]
    private GameObject _questPrefab;

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

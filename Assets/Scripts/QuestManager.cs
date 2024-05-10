using System.Collections.Generic;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    [SerializeField]
    private QuestData[] _quests;

    private Dictionary<QuestData, QuestState> _questStates = new();

    public Quest CurrentQuest;

    protected override void Awake()
    {
        base.Awake();

        foreach (QuestData quest in _quests)
        {
            _questStates.Add(quest, new());
        }
    }

    private void OnEnable()
    {
        EventManager.Instance.QuestAccepted += StartQuest;
        EventManager.Instance.QuestRewardClaimed += OnQuestCompleted;
        EventManager.Instance.AfterQuestUnlocked += OnQuestUnlocked;
    }

    private void OnQuestUnlocked(QuestData quest)
    {
        _questStates[quest].IsUnlocked = true;
    }

    private void OnQuestCompleted(QuestData quest)
    {
        Instantiate(CurrentQuest.Data.Reward);
        CurrentQuest = null;
        _questStates[quest].IsRewardClaimed = true;
    }

    public void StartQuest(QuestData quest)
    {
        if (CurrentQuest != null) return;
        CurrentQuest = new()
        {
            Data = quest
        };
        CurrentQuest.StartQuest();
    }

    public bool IsUnlocked(QuestData quest)
    {
        return _questStates[quest].IsUnlocked;
    }

    public bool IsCompleted(QuestData quest)
    {
        return _questStates[quest].IsRewardClaimed;
    }
}

public class QuestState
{
    public bool IsUnlocked = false;
    public bool IsRewardClaimed = false;
}

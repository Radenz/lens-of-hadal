using System;
using System.Collections.Generic;
using System.Linq;
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

    private void Start()
    {
        StartFirstQuest();
    }

    private async void StartFirstQuest()
    {
        await Awaitable.NextFrameAsync();
        EventManager.Instance.UnlockQuest(_quests[0]);
        await Awaitable.NextFrameAsync();
        EventManager.Instance.AcceptQuest(_quests[0]);
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

        int index = Array.IndexOf(_quests, quest);
        if (index == -1)
        {
            Debug.LogError("Illegal quest");
            return;
        }

        if (_quests.Length == index + 1) return;
        EventManager.Instance.UnlockQuest(_quests[index + 1]);
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

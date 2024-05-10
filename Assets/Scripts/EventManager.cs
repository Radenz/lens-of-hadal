using System;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    #region Currency Events
    public event Action<int, int> GoldChanged;
    public event Action<int, int> SeaweedChanged;
    public event Action<int, int> ScrapMetalChanged;
    public event Action<int, int> EnergyPowderChanged;
    #endregion

    #region Quest Events
    public event Action<string, string> DisplayQuestChanged;
    public event Action DisplayQuestHidden;
    public event Action<QuestData> QuestCompleted;
    public event Action<QuestData> QuestUnlocked;
    public event Action<QuestData> QuestAccepted;
    public event Action<QuestData> QuestRewardClaimed;
    #endregion

    public void SetGold(int initialValue, int finalValue)
    {
        GoldChanged?.Invoke(initialValue, finalValue);
    }

    public void SetSeaweed(int initialValue, int finalValue)
    {
        SeaweedChanged?.Invoke(initialValue, finalValue);
    }

    public void SetScrapMetal(int initialValue, int finalValue)
    {
        ScrapMetalChanged?.Invoke(initialValue, finalValue);
    }

    public void SetEnergyPowder(int initialValue, int finalValue)
    {
        EnergyPowderChanged?.Invoke(initialValue, finalValue);
    }

    public void SetQuestDisplay(string title, string description)
    {
        DisplayQuestChanged?.Invoke(title, description);
    }

    public void UnlockQuest(QuestData quest)
    {
        QuestUnlocked?.Invoke(quest);
    }

    public void AcceptQuest(QuestData quest)
    {
        QuestAccepted?.Invoke(quest);
    }

    public void ClaimQuestReward(QuestData quest)
    {
        QuestRewardClaimed?.Invoke(quest);
    }

    public void CompleteQuest(QuestData quest)
    {
        QuestCompleted?.Invoke(quest);
    }

    public void HideDisplayQuest()
    {
        DisplayQuestHidden?.Invoke();
    }
}

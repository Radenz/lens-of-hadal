using System;
using UnityEngine;

// ? Hardcoding available events are not ideal, but
// it works & requires less time. So be it.
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
    public event Action<QuestData> AfterQuestUnlocked;
    public event Action<QuestData> QuestAccepted;
    public event Action<QuestData> QuestRewardClaimed;
    #endregion

    #region Levelling Events
    public event Action<int> ExpChanged;
    public event Action<int> LevelledUp;
    #endregion

    #region Shop Events
    public event Action<string> ModuleUnlocked;
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
        AfterQuestUnlocked?.Invoke(quest);
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
        if (QuestManager.Instance.CurrentQuest == null) return;
        if (QuestManager.Instance.CurrentQuest.Data != quest) return;
        QuestCompleted?.Invoke(quest);
    }

    public void HideDisplayQuest()
    {
        DisplayQuestHidden?.Invoke();
    }

    public void ChangeExp(int exp)
    {
        ExpChanged?.Invoke(exp);
    }

    public void LevelUp(int level)
    {
        LevelledUp?.Invoke(level);
    }
}

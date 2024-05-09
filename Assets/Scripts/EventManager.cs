using System;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    #region Currency Events
    public event Action<int> GoldChanged;
    public event Action<int> SeaweedChanged;
    public event Action<int> ScrapMetalChanged;
    public event Action<int> EnergyPowderChanged;
    #endregion

    #region Quest Events
    public event Action<string, string> DisplayQuestChanged;
    public event Action DisplayQuestHidden;
    public event Action QuestCompleted;
    #endregion

    public void SetGold(int quantity)
    {
        GoldChanged?.Invoke(quantity);
    }

    public void SetSeaweed(int quantity)
    {
        SeaweedChanged?.Invoke(quantity);
    }

    public void SetScrapMetal(int quantity)
    {
        ScrapMetalChanged?.Invoke(quantity);
    }

    public void SetEnergyPowder(int quantity)
    {
        EnergyPowderChanged?.Invoke(quantity);
    }

    public void SetQuestDisplay(string title, string description)
    {
        DisplayQuestChanged?.Invoke(title, description);
    }

    public void CompleteQuest()
    {
        QuestCompleted?.Invoke();
    }

    public void HideDisplayQuest()
    {
        DisplayQuestHidden?.Invoke();
    }
}

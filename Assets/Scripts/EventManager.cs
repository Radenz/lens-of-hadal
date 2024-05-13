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
    public event Action<int, int, int> Rewarded;
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
    public event Action<string> ShopItemShown;
    public event Action<string> ShopItemUnlocked;
    public event Action<string> ShopItemAssembled;
    #endregion

    #region Consumables Events
    public event Action<int> FlashlightEquipped;
    public event Action FlashlightUnequipped;
    public event Action<int> ScannerEquipped;
    public event Action ScannerUnequipped;

    public event Action<int> SonarQuantityChanged;
    public event Action<int> FlareQuantityChanged;


    public event Action SonarPinged;
    #endregion

    #region Creature Events
    public event Action CreaturesDisabled;
    public event Action CreaturesEnabled;
    #endregion

    #region Player Events
    public event Action PlayerActionsDisabled;
    public event Action PlayerActionsEnabled;
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

    public void PingSonar()
    {
        SonarPinged?.Invoke();
    }

    // FIX: rename terminology to buy blueprint
    public void UnlockItem(string id)
    {
        ShopItemShown?.Invoke(id);
    }

    public void AssembleItem(string id)
    {
        ShopItemAssembled?.Invoke(id);
    }

    public void ChangeSonarQuantity(int quantity)
    {
        SonarQuantityChanged?.Invoke(quantity);
    }

    public void ChangeFlareQuantity(int quantity)
    {
        FlareQuantityChanged?.Invoke(quantity);
    }

    public void EquipItem(string id)
    {
        switch (id)
        {
            case "Flashlight2":
                FlashlightEquipped?.Invoke(2);
                break;
            case "Flashlight3":
                FlashlightEquipped?.Invoke(3);
                break;
            case "Scanner2":
                ScannerEquipped?.Invoke(2);
                break;
            case "Scanner3":
                ScannerEquipped?.Invoke(3);
                break;
        }
    }

    public void UnequipItem(string id)
    {
        switch (id)
        {
            case "Flashlight2":
            case "Flashlight3":
                FlashlightUnequipped?.Invoke();
                break;
            case "Scanner2":
            case "Scanner3":
                ScannerUnequipped?.Invoke();
                break;
        }
    }

    public void DisableCreatures()
    {
        CreaturesDisabled?.Invoke();
    }

    public void EnableCreatures()
    {
        CreaturesEnabled?.Invoke();
    }

    public void DisablePlayerActions()
    {
        PlayerActionsDisabled?.Invoke();
    }

    public void EnablePlayerActions()
    {
        PlayerActionsEnabled?.Invoke();
    }

    public void RewardPlayer(int energyPowder, int seaweed, int scrapMetal)
    {
        Rewarded?.Invoke(energyPowder, seaweed, scrapMetal);
    }
}

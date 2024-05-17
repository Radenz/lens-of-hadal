using System;
using Common.Persistence;

public class ItemData : ISaveable
{
    public UpgradableItemData FlashlightLv2 = new();
    public UpgradableItemData FlashlightLv3 = new();
    public UpgradableItemData ScannerLv2 = new();
    public UpgradableItemData ScannerLv3 = new();
    public UpgradableItemData DivingSuitLv2 = new();
    public UpgradableItemData DivingSuitLv3 = new();
    public ItemInstanceData Flare = new();
    public ItemInstanceData SonarDrone = new();

    public ItemInstanceData FromId(string name)
    {
        return name switch
        {
            ShopItems.DivingSuitLv2 => DivingSuitLv2,
            ShopItems.DivingSuitLv3 => DivingSuitLv3,
            ShopItems.FlashlightLv2 => FlashlightLv2,
            ShopItems.FlashlightLv3 => FlashlightLv3,
            ShopItems.ScannerLv2 => ScannerLv2,
            ShopItems.ScannerLv3 => ScannerLv3,
            ShopItems.Flare => Flare,
            ShopItems.SonarDrone => SonarDrone,
            _ => throw new ArgumentException("Invalid item id")
        };
    }
}


public class ItemInstanceData
{
    public bool IsUnlocked = false;
    public bool IsPurchased = false;
}

public class UpgradableItemData : ItemInstanceData
{
    public bool IsAssembled = false;
}


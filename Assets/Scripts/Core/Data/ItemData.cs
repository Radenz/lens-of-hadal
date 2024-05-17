public class ItemData
{
    public UpgradableItemData FlashlightLv2;
    public UpgradableItemData FlashlightLv3;
    public UpgradableItemData ScannerLv2;
    public UpgradableItemData ScannerLv3;
    public UpgradableItemData DivingSuitLv2;
    public UpgradableItemData DivingSuitLv3;
    public ConsumableItemData Flare;
    public ConsumableItemData SonarDrone;
}

public class UpgradableItemData
{
    public bool IsUnlocked = false;
    public bool IsBought = false;
    public bool IsAssembled = false;
}

public class ConsumableItemData
{
    public bool IsUnlocked = false;
    public bool IsBought = false;
}

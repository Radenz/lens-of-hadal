using Common.Persistence;

public class ConsumablesManager : Singleton<ConsumablesManager>, IBind<ConsumableData>
{
    private ConsumableData _data;

    public int SonarDrone
    {
        get => _data.SonarDrone;
        set
        {
            _data.SonarDrone = value;
            EventManager.Instance.ChangeSonarQuantity(value);
        }
    }

    public int Flare
    {
        get => _data.Flare;
        set
        {
            _data.Flare = value;
            EventManager.Instance.ChangeFlareQuantity(value);
        }
    }

    private void Start()
    {
        EventManager.Instance.ShopItemUnlocked += OnItemUnlocked;
        EventManager.Instance.ShopItemPurchased += OnItemPurchased;
    }

    void IBind<ConsumableData>.Bind(ConsumableData data)
    {
        _data = data;
    }

    private void OnItemUnlocked(string id)
    {
        switch (id)
        {
            case ShopItems.Flare:
                _data.IsFlareUnlocked = true;
                break;
            case ShopItems.SonarDrone:
                _data.IsFlareUnlocked = true;
                break;
        }
    }

    private void OnItemPurchased(string id)
    {
        switch (id)
        {
            case ShopItems.Flare:
                _data.IsFlareBought = true;
                break;
            case ShopItems.SonarDrone:
                _data.IsSonarDroneBought = true;
                break;
        }
    }
}

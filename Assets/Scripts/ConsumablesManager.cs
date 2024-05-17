using Common.Persistence;

public class ConsumablesManager : Singleton<ConsumablesManager>, IBind<ConsumableData>
{
    private ConsumableData _consumableData;

    public int SonarDrone
    {
        get => _consumableData.SonarDrone;
        set
        {
            _consumableData.SonarDrone = value;
            EventManager.Instance.ChangeSonarQuantity(value);
        }
    }

    public int Flare
    {
        get => _consumableData.Flare;
        set
        {
            _consumableData.Flare = value;
            EventManager.Instance.ChangeFlareQuantity(value);
        }
    }

    void IBind<ConsumableData>.Bind(ConsumableData data)
    {
        _consumableData = data;
    }
}

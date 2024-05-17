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

    void IBind<ConsumableData>.Bind(ConsumableData data)
    {
        _data = data;
    }
}

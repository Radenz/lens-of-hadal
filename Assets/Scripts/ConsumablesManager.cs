public class ConsumablesManager : Singleton<ConsumablesManager>
{
    private int _sonarDrone = 0;
    private int _flare = 0;

    public int SonarDrone
    {
        get => _sonarDrone;
        set
        {
            _sonarDrone = value;
            EventManager.Instance.ChangeSonarQuantity(value);
        }
    }

    public int Flare
    {
        get => _flare;
        set
        {
            _flare = value;
            EventManager.Instance.ChangeFlareQuantity(value);
        }
    }
}

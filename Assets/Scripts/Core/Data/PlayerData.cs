using Common.Persistence;

// TODO: add exp & level
public class PlayerData : ISaveable
{
    public float MaxHealthPoints = 100;
    public float HealthPoints = 100;
    public float MaxStamina = 50;
    public float Stamina = 50;

    public int FlashlightLevel = 1;
    public int ScannerLevel = 1;
}

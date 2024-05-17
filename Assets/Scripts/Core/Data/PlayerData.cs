using Common.Persistence;

// TODO: scanner level
public class PlayerData : ISaveable
{
    public float MaxHealthPoints = 100;
    public float HealthPoints = 100;
    public float MaxStamina = 100;
    public float Stamina = 100;

    public int FlashlightLevel = 1;
}

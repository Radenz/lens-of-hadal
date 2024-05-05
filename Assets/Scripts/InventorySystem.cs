// ? This class is used to store consumables
public class InventorySystem : Singleton<InventorySystem>
{
    public int SonarDrone;
    public int Flare;

    public void AddSonarDrone(int quantity)
    {
        SonarDrone += quantity;
    }

    public void AddFlare(int quantity)
    {
        Flare += quantity;
    }
}

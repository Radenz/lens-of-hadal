using Common.Persistence;

public class BestiaryManager : Singleton<BestiaryManager>, IBind<CreatureData>
{
    private CreatureData _data;

    private void Start()
    {
        EventManager.Instance.CreatureDNAIncreased += OnCreatureDNAIncreased;
    }

    void IBind<CreatureData>.Bind(CreatureData data)
    {
        _data = data;
    }

    private void OnCreatureDNAIncreased(string name, float dna)
    {
        CreatureInstanceData creatureData = _data.FromName(name);
        creatureData.DiscoveryProgress += dna;
        if (creatureData.DiscoveryProgress >= 100)
        {
            creatureData.DiscoveryProgress = 100;
            creatureData.IsDiscovered = true;
            EventManager.Instance.DiscoverCreature(name);
        }
    }
}

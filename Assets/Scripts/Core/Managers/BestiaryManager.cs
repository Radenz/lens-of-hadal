using Common.Persistence;
using UnityEngine;

public class BestiaryManager : Singleton<BestiaryManager>, IBind<CreatureData>
{
    private CreatureData _data;

    [SerializeField]
    private Bestiary _bestiary;

    private void Start()
    {
        EventManager.Instance.DNAGained += OnCreatureDNAGained;
        EventManager.Instance.CreatureDiscovered += OnCreatureDiscovered;
    }

    void IBind<CreatureData>.Bind(CreatureData data)
    {
        _data = data;
        _bestiary.Bind(data);
    }

    private void OnCreatureDNAGained(Creature creature, float dna)
    {
        CreatureInstanceData creatureData = _data.FromId(creature.Id);
        creatureData.DiscoveryProgress += dna;
        if (creatureData.DiscoveryProgress >= 100)
        {
            creatureData.DiscoveryProgress = 100;
            creatureData.IsDiscovered = true;

            Announcer.Instance.AnnounceDiscovery(creature);
        }
    }

    private void OnCreatureDiscovered(string name)
    {
        _data.FromId(name).IsDiscovered = true;
    }
}

using System;
using Common.Persistence;

public class CreatureData : ISaveable
{
    public CreatureInstanceData Piranha = new();
    public CreatureInstanceData ElectricEel = new();
    public CreatureInstanceData Pufferfish = new();
    public CreatureInstanceData GiantSquid = new();
    public CreatureInstanceData Bladefish = new();
    public CreatureInstanceData MutantAnglerfish = new();

    public CreatureInstanceData FromId(string id)
    {
        return id switch
        {
            Creatures.Piranha => Piranha,
            Creatures.ElectricEel => ElectricEel,
            Creatures.Pufferfish => Pufferfish,
            Creatures.GiantSquid => GiantSquid,
            Creatures.Bladefish => Bladefish,
            Creatures.MutantAnglerfish => MutantAnglerfish,
            Creatures.Kraken => null,
            _ => throw new ArgumentException($"Invalid creature id: {id}")
        };
    }
}

public class CreatureInstanceData
{
    public float DNAGathered = 0f;
    public bool IsDiscovered = false;
    public float DiscoveryProgress = 0f;
}

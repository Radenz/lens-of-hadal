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

    public CreatureInstanceData FromName(string name)
    {
        return name switch
        {
            Creatures.Piranha => Piranha,
            Creatures.ElectricEel => ElectricEel,
            Creatures.Pufferfish => Pufferfish,
            Creatures.GiantSquid => GiantSquid,
            Creatures.Bladefish => Bladefish,
            Creatures.MutantAnglerfish => MutantAnglerfish,
            _ => throw new ArgumentException("Invalid creature name")
        };
    }
}

public class CreatureInstanceData
{
    public bool IsDiscovered = false;
    public float DiscoveryProgress = 0f;
}

using System.Collections.Generic;
using UnityEngine;

public class CreatureManager : Singleton<CreatureManager>
{
    [field: SerializeField]
    public int MaxCreatures { get; private set; }
    [field: SerializeField]
    public float SpawnRadius { get; private set; }
    [field: SerializeField]
    public float DespawnRadius { get; private set; }

    private readonly List<CreatureDespawner> _creatures = new();
    public int CreatureCount => _creatures.Count;

    public void Register(CreatureDespawner creature)
    {
        _creatures.Add(creature);
    }

    public void Unregister(CreatureDespawner creature)
    {
        _creatures.Remove(creature);
    }
}

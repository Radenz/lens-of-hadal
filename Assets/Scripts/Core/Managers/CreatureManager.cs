using System.Collections.Generic;
using NaughtyAttributes;
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
    public List<CreatureDespawner> Creatures => _creatures;

    public void LogCount()
    {
        Debug.Log(CreatureCount);
    }

    public void Register(CreatureDespawner creature)
    {
        _creatures.Add(creature);
    }

    public void Unregister(CreatureDespawner creature)
    {
        _creatures.Remove(creature);
    }
}

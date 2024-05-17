using System;
using NaughtyAttributes;
using UnityEngine;

public class Cheat : MonoBehaviour
{
    public int Exp;

    [Button]
    public void GiveExp()
    {
        LevelManager.Instance.AddExp(Exp);
    }

    public int Gold;
    public int EnergyPowder;
    public int Seaweed;
    public int ScrapMetal;

    [Button]
    public void GiveReward()
    {
        CurrencySystem.Instance.Gold += Gold;
        CurrencySystem.Instance.EnergyPowder += EnergyPowder;
        CurrencySystem.Instance.Seaweed += Seaweed;
        CurrencySystem.Instance.ScrapMetal += ScrapMetal;
    }

    public string CreatureId;

    [Button]
    public void DiscoverCreature()
    {
        EventManager.Instance.DiscoverCreature(CreatureId);
    }
}

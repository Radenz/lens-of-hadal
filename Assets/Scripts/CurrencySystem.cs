// TODO: hook up with UI
using System;
using NaughtyAttributes;
using UnityEngine;

public class CurrencySystem : Singleton<CurrencySystem>
{
    [SerializeField]
    private int _gold;
    [SerializeField]
    private int _energyPowder;
    [SerializeField]
    private int _seaweed;
    [SerializeField]
    private int _scrapMetal;

    public int Gold
    {
        get => _gold;
        set
        {
            int initialValue = _gold;
            _gold = value;
            EventManager.Instance.SetGold(initialValue, value);
        }
    }

    public int EnergyPowder
    {
        get => _energyPowder;
        set
        {
            int initialValue = _energyPowder;
            _energyPowder = value;
            EventManager.Instance.SetEnergyPowder(initialValue, value);
        }
    }

    public int Seaweed
    {
        get => _seaweed;
        set
        {
            int initialValue = _seaweed;
            _seaweed = value;
            EventManager.Instance.SetSeaweed(initialValue, value);
        }
    }

    public int ScrapMetal
    {
        get => _scrapMetal;
        set
        {
            int initialValue = _scrapMetal;
            _scrapMetal = value;
            EventManager.Instance.SetScrapMetal(initialValue, value);
        }
    }
}

[Serializable]
public enum MaterialKind
{
    EnergyPowder,
    Seaweed,
    ScrapMetal
}

// TODO: hook up with UI
using System;

public class CurrencySystem : Singleton<CurrencySystem>
{
    private int _gold;
    private int _energyPowder;
    private int _seaweed;
    private int _scrapMetal;

    public int Gold
    {
        get => _gold;
        set
        {
            _gold = value;
            EventManager.Instance.SetGold(value);
        }
    }

    public int EnergyPowder
    {
        get => _energyPowder;
        set
        {
            _energyPowder = value;
            EventManager.Instance.SetEnergyPowder(value);
        }
    }

    public int Seaweed
    {
        get => _seaweed;
        set
        {
            _seaweed = value;
            EventManager.Instance.SetSeaweed(value);
        }
    }

    public int ScrapMetal
    {
        get => _scrapMetal;
        set
        {
            _scrapMetal = value;
            EventManager.Instance.SetScrapMetal(value);
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

using System;
using NaughtyAttributes;

public static class QuestValidators
{

}

[Serializable]
public class QuestValidator
{
    public ValidatorKind Type;

    [AllowNesting, ShowIf("IsCountable")]
    public int Quantity;

    [AllowNesting, ShowIf("IsNamed")]
    public string Name;

    private bool IsCountable()
    {
        return Type == ValidatorKind.CountSeaweed
            || Type == ValidatorKind.CountScrapMetal
            || Type == ValidatorKind.CountScrapMetal;
    }

    private bool IsNamed()
    {
        return Type == ValidatorKind.ScanCreature;
    }
}


public enum ValidatorKind
{
    CountSeaweed,
    CountScrapMetal,
    CountEnergyPowder,
    ScanCreature
}

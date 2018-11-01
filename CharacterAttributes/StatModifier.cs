[System.Serializable]
public enum StatModType
{
    Flat = 100,
    PercentageAdd = 200,
    PercentageMultiplicative = 300
}

public enum OrderType
{
    Base = 100,
    Talent = 200,
    Buff = 300
}


[System.Serializable]
public class StatModifier
{
    public StatType StatType;
    public StatModType Type;
    public float Value;

    public OrderType OrderType;
    public IStatModifierSource Source;

    public StatModifier(float value, StatModType type)
    {
        Value = value;
        Type = type;
    }
}

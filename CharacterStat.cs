using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

[Serializable]
public enum StatType
{
    Strength, Vitality
}

[Serializable]
public class CharacterStat
{
    public StatType Type;
    public float BaseValue;

    public virtual float Value
    {
        get
        {
            if (isDirty || BaseValue != _lastBaseValue)
            {
                _lastBaseValue = BaseValue;
                _value = CalculateFinalValue();
                isDirty = false;
            }

            return _value;
        }
    }

    protected bool isDirty = true;

    protected float _value;
    protected float _lastBaseValue = float.MinValue;

    protected readonly List<StatModifier> statModifiers;
    protected readonly ReadOnlyCollection<StatModifier> StatModifiers;

    public CharacterStat()
    {
        statModifiers = new List<StatModifier>();
        StatModifiers = statModifiers.AsReadOnly();
    }

    public CharacterStat(float baseValue)
    {
        BaseValue = baseValue;
        statModifiers = new List<StatModifier>();
        StatModifiers = statModifiers.AsReadOnly();
    }

    public virtual void AddModifier(StatModifier mod)
    {
        statModifiers.Add(mod);
        isDirty = true;
    }

    public virtual bool RemoveModifier(StatModifier mod)
    {
        if (statModifiers.Remove(mod))
        {
            isDirty = true;
            return true;
        }
        return false;
    }

    public virtual void AddAllModifierFromSource(IStatModifierSource source)
    {
        foreach (var mod in source.GetAllModifiers().Where(w => w.StatType == Type))
        {
            statModifiers.Add(mod);
        }
    }

    public virtual int RemoveAllModifiersFromSource(IStatModifierSource source)
    {
        isDirty = true;
        return statModifiers.RemoveAll(w => w.Source == source);
    }

    protected virtual float CalculateFinalValue()
    {
        float finalValue = BaseValue;
        float sumPercentageAdd = 0;

        foreach (var mod in statModifiers.OrderBy(o => o.OrderType))
        {
            if (mod.Type == StatModType.Flat)
            {
                finalValue += mod.Value;
            }
            else if (mod.Type == StatModType.PercentageAdd)
            {
                sumPercentageAdd += mod.Value;
            }
            else if (mod.Type == StatModType.PercentageMultiplicative)
            {
                finalValue *= 1 + mod.Value;
            }

        }
        finalValue *= 1 + sumPercentageAdd;


        return (float)Math.Round(finalValue, 4);
    }
}

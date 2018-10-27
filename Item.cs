using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IStatModifierSource
{
    public List<StatModifier> StatModifiers;

    private void Awake()
    {
        foreach(var mod in StatModifiers)
        {
            mod.Source = this;
        }
    }

    public List<StatModifier> GetAllModifiers()
    {
        return StatModifiers;        
    }
}

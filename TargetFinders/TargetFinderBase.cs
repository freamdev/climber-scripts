using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TargetFinderBase : ScriptableObject, ITargetFinder
{
    public abstract GameCharacter GetTarget(bool isPlayer, GameCharacter me);
}

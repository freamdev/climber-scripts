using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ClosestTarget", menuName = "TargetFinders/ClosestTarget", order = 1)]
public class FindNearestCharacter : TargetFinderBase
{
    public override GameCharacter GetTarget(bool isPlayer, GameCharacter me)
    {
        if (isPlayer)
        {
            return BattleCacheSingleton.EnemyCharacters.OrderBy(o => Vector3.Distance(o.transform.position, me.transform.position)).First();
        }
        else
        {
            return BattleCacheSingleton.PlayerCharacters.OrderBy(o => Vector3.Distance(o.transform.position, me.transform.position)).First();
        }
    }

}

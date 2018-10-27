using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyCharacter : GameCharacter {
    protected override GameCharacter GetTarget()
    {
        return FindObjectsOfType<PlayerCharacter>().First();
    }

    private void Awake()
    {
        InitStats();
    }

    private void Update()
    {
        base.Iterate();
    }


}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerCharacter : GameCharacter {
    protected override GameCharacter GetTarget()
    {
        return FindObjectsOfType<EnemyCharacter>().First();
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

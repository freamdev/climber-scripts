using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetFinder {

    GameCharacter GetTarget(bool isPlayer, GameCharacter me);

}

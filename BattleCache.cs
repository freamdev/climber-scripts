using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleCache : MonoBehaviour
{
    public float FastestAttackTime;

    // Use this for initialization
    void Start()
    {
        BattleCacheSingleton.PlayerCharacters = GameObject.FindGameObjectsWithTag("Player").Select(s => s.gameObject.GetComponent<GameCharacter>()).ToList();
        BattleCacheSingleton.EnemyCharacters = GameObject.FindGameObjectsWithTag("Enemy").Select(s => s.gameObject.GetComponent<GameCharacter>()).ToList();
        BattleCacheSingleton.FastestAttackTime = FastestAttackTime;
    }


}

public static class BattleCacheSingleton
{
    public static List<GameCharacter> PlayerCharacters;
    public static List<GameCharacter> EnemyCharacters;
    public static float FastestAttackTime;
}



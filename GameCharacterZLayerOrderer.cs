using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameCharacterZLayerOrderer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var enemies = GameObject.FindObjectsOfType<GameCharacter>().ToList();
        enemies = enemies.OrderBy(o => o.transform.position.y).ToList();
        for(int i = 0; i < enemies.Count; i++)
        {
            enemies[i].OrderLayer(i);
        }
	}
}

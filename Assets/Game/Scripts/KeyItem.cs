using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItem : MonoBehaviour {

    private void OnTriggerEnter(Collider collider) 
    {
        if(collider.gameObject.name == "Person") //Collider detects if a player has entered the box collider in close proximity to the key. 
        {
            GameVariables.keyCount += 2;
            Destroy(gameObject); //Once the player has the key, it removes it from the ground to mimic picking up the key.
        }
    }
}

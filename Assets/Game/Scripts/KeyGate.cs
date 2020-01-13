using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyGate : MonoBehaviour {
 
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Person" && GameVariables.keyCount > 0) //Collider detects if a player has entered the collider zone holding the key.
        {
            GameVariables.keyCount--; 
            Destroy(gameObject); //Removes the gate blocking player from entering the castle.
        }
    }
}

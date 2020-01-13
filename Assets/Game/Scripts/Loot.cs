using System;
using System.Linq;
using UnityEngine;

public class Loot : MonoBehaviour {

	int numOfStacks;
    public Inventory inv;


	// Use this for initialization
	void Start () {
		//1 & 2 are more likely
		numOfStacks = UnityEngine.Random.Range(10, 34) / 10;
        for (int i = 0; i < numOfStacks; i++)
        {
            inv.AddItem(EquippableItem.GenRand());
        }

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

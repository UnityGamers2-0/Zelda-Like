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
        //types = new ItemTypes[numOfStacks];
        for (int i = 0; i < numOfStacks; i++)
        {
            //Essentially gets a random item type (that isn't key)
            //types[i] = (ItemTypes)UnityEngine.Random.Range(0, (int)ItemTypes.Key - 1);
			//inv.AddItem();
        }

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

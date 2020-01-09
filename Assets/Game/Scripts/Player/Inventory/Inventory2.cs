using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory2 {

    private ItemStack[,] inventory;

    public Inventory2(int width, int height)
    {
        inventory = new ItemStack[width,height];
    }

    //returns whether or not the item can be added
    public bool AddItem(ItemStack items)
    {
        foreach (ItemStack stack in inventory)
        {
            if (stack.AddItems(items))
            {
                return true;
            }
        }
        return false;
    }

    public void RemoveItems(int x, int y)
    {

    }
}

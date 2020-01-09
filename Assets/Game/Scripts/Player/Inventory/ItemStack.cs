using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//More can be added, however, Key must be last
public enum ItemTypes
{
    Helmet,
    Chestplate,
    Leggings,
    Boots,
    Arrow,
    Upgrade,
    Key
}


public class ItemStack {

	string name;
	int items;
	bool stackable;
	float protection;

	//num should be an int if stackable is true
	public ItemStack(string name, float num = 0, bool stackable = true)
    {
		this.name = name;
		this.stackable = stackable;
		if (stackable)
		{
			items = (int)num;
		}
		else
		{
			protection = num;
		}
    }

	public bool AddItems(ItemStack itemStack)
    {
		if (name == "")
        {
            name = itemStack.name;
            stackable = itemStack.stackable;
            items = itemStack.items;
            protection = itemStack.protection;
            return true;
        }
        else if (itemStack.name == name && itemStack.stackable)
        {
			items += itemStack.items;
            return true;
        }
        else
        {
            return false;
        }
    }

	public void Remove()
    {
		name = "";
		items = 0;
    }

    public bool IsStackable()
    {
        return stackable;
    }

    public static ItemStack GenerateItemStack(ItemTypes type, bool isCorrect = false)
    {
        string name;
        int num = 0;
        bool stacks;
        if (type != ItemTypes.Arrow)
        {
            //"adjective", weight, modifier
            object[,] adjs = { { "Ludicrous", 1, 8 }, { "Terrific", 5, 4 }, { "Magical", 10, 2 }, { "Dynamic", 15, 1 }, { "Normal", 20, 0 }, { "Small", 15, -1 }, { "Flimsy", 10, -2 }, { "Hollow", 5, -4 }, { "Terrible", 1, -8 } };
            string adj = "";
            int totalWeight = 0;
            foreach (int a in adjs)
            {
                totalWeight += a;
            }
            int quality = Random.Range(0, totalWeight);
            int mod = 0;
            Debug.Log("Quality: " + quality + "; tWeight: " + totalWeight);
            for (int i = 0; i < adjs.Length; i++)
            {
                quality -= (int)adjs[i, 1];
                if (quality <= 0)
                {
                    adj = (string)adjs[i, 0];
                    mod = (int)adjs[i, 2];
                }
            }

            name = adj + type.ToString();
            stacks = false;
            if (type == ItemTypes.Helmet)
            {
                num = Random.Range(5 + (mod / 2), 15 + mod);
            }
            else if (type == ItemTypes.Chestplate)
            {
                num = Random.Range(7 + (mod / 2), 17 + mod);
            }
            else if (type == ItemTypes.Leggings)
            {
                num = Random.Range(6 + (mod / 2), 16 + mod);
            }
            else if (type == ItemTypes.Boots)
            {
                num = Random.Range(4 + (mod / 2), 14 + mod);
            }
            else if (type == ItemTypes.Upgrade)
            {
                num = Random.Range(4 + (mod / 2), 10 + mod);
            }
            else if (type == ItemTypes.Key)
            {
                num = isCorrect ? 1 : 0;
            }
            else
            {
                Debug.LogError("The ItemType defined is not recognized");
            }
        }
        else
        {
            name = type.ToString();
            num = Random.Range(1, 10);
            stacks = true;
        }

        return new ItemStack(name, num, stacks);
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EquipmentType
{
    Helmet,
    Chestplate,
    Leggings,
    Boots,
    Weapon1,
    Weapon2
}

[CreateAssetMenu]
public class EquippableItem : Item
{
    static List<Sprite> helms = new List<Sprite>();
    static List<Sprite> chests = new List<Sprite>();
    static List<Sprite> legs = new List<Sprite>();
    static List<Sprite> boots = new List<Sprite>();
    static List<Sprite> swords = new List<Sprite>();
    static List<Sprite> sheilds = new List<Sprite>();
    static List<Sprite> bows = new List<Sprite>();
    static List<Sprite> gloves = new List<Sprite>();
    static List<Sprite> wands = new List<Sprite>();
    bool loaded = false;

    public int AttackBonus;
    public int AgillityBonus;
    public int DefenseBonus;
    [Space]
    public EquipmentType EquipmentType;

    private void Awake()
    {
        if (!loaded)
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>("Item Icons");
            foreach (Sprite s in sprites)
            {
                if (s.name.Contains("helmet"))
                {
                    helms.Add(s);
                }
                else if (s.name.Contains("chestplate"))
                {
                    chests.Add(s);
                }
                else if (s.name.Contains("leggings"))
                {
                    legs.Add(s);
                }
                else if (s.name.Contains("boots"))
                {
                    boots.Add(s);
                }
                else if (s.name.Contains("sword"))
                {
                    swords.Add(s);
                }
                else if (s.name.Contains("sheild"))
                {
                    sheilds.Add(s);
                }
                else if (s.name.Contains("bow"))
                {
                    bows.Add(s);
                }
                else if (s.name.Contains("gloves"))
                {
                    gloves.Add(s);
                }
                else if (s.name.Contains("wand"))
                {
                    wands.Add(s);
                }
            }
        }
    }

    public static EquippableItem GenRand()
    {
        Player.Class c = Player.pClass;

        EquippableItem item = CreateInstance<EquippableItem>();
        //Essentially gets a random item type
        EquipmentType type = (EquipmentType)Random.Range(0, 5);
        //"adjective", weight, modifier, image
        object[,] adjs = { { "Ludicrous", 1, 8, 3 }, { "Terrific", 5, 4, 3 }, { "Magical", 10, 2, 2 }, { "Dynamic", 15, 1, 2 }, { "Normal", 20, 0, 1 }, { "Small", 15, -1, 1 }, { "Flimsy", 10, -2, 1 }, { "Hollow", 5, -4, 0 }, { "Terrible", 1, -8, 0 } };
        string adj = "";
        int totalWeight = 0;
        for (int i = 0; i < adjs.GetLength(0); i++)
        {
            totalWeight += (int)adjs[i, 1];
        }
        int quality = Random.Range(0, totalWeight);
        int mod = 0;
        int img = 0;
        bool found = false;

        for (int i = 0; i < adjs.GetLength(0); i++)
        {
            quality -= (int)adjs[i, 1];
            if (!found && quality <= 0)
            {
                adj = (string)adjs[i, 0];
                mod = (int)adjs[i, 2];
                img = (int)adjs[i, 3];
                found = true;
            }
        }

        string itemType = "";
        if (type == EquipmentType.Weapon1)
        {
            if (c == Player.Class.Archer)
            {
                itemType = "Bow";
                item.Icon = bows[img / 2];
            }
            else if (c == Player.Class.Knight)
            {
                itemType = "Sword";
                item.Icon = swords[img];
            }
            else if (c == Player.Class.Mage)
            {
                itemType = "Wand";
                item.Icon = wands[img / 2];
            }
        }
        else if (type == EquipmentType.Weapon2)
        {
            if (c == Player.Class.Archer)
            {
                itemType = "Gloves";
                item.Icon = gloves[img / 2];
            }
            else if (c == Player.Class.Knight)
            {
                itemType = "Sheild";
                item.Icon = sheilds[img];
            }
            else if (c == Player.Class.Mage)
            {
                itemType = "Wand";
                item.Icon = wands[img / 2];
            }
        }
        else
        {
            itemType = type.ToString();
            if (type == EquipmentType.Helmet)
            {
                item.Icon = helms[img / 2];
            }
            else if (type == EquipmentType.Chestplate)
            {
                item.Icon = chests[img / 2];
            }
            else if (type == EquipmentType.Leggings)
            {
                item.Icon = legs[img / 2];
            }
            else if (type == EquipmentType.Boots)
            {
                item.Icon = boots[img / 2];
            }
        }

        item.name = adj + " " + itemType;
        item.ItemName = adj + " " + itemType;
        item.EquipmentType = type;
        

        switch (type)
        {
            case EquipmentType.Helmet:
                item.DefenseBonus = Random.Range(5 + (mod / 2), 15 + mod);
                break;
            case EquipmentType.Chestplate:
                item.DefenseBonus = Random.Range(7 + (mod / 2), 17 + mod);
                break;
            case EquipmentType.Leggings:
                item.DefenseBonus = Random.Range(6 + (mod / 2), 16 + mod);
                break;
            case EquipmentType.Boots:
                item.DefenseBonus = Random.Range(4 + (mod / 2), 14 + mod);
                break;
            case EquipmentType.Weapon1:
            case EquipmentType.Weapon2:
                item.AttackBonus = Random.Range(4 + (mod / 2), 10 + mod);
                break;
            default:
                Debug.LogError("The EquipmentType defined is not recognized");
                break;
        }

        item.AttackBonus = Random.Range(0, mod);
        item.AgillityBonus = Random.Range(0, mod);

        return item;
    }
}
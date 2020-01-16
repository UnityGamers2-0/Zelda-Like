using System;
using UnityEngine;

public class EquipmentPanel : MonoBehaviour 
{
    [SerializeField] Transform equipmentSlotsParent;
    [SerializeField] EquipmentSlot[] equipmentSlots;
    [Space]
    [SerializeField] StatDisplay attack;
    [SerializeField] StatDisplay agility;
    [SerializeField] StatDisplay defense;
    [Space]
    [SerializeField] Player player;

    public event Action<Item> OnItemRightClickedEvent;
    public event Action<Item> OnEnter;
    public event Action<Item> OnExit;

    private void Awake()
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            equipmentSlots[i].OnRightClickEvent += OnItemRightClickedEvent;
            equipmentSlots[i].OnEnter += OnEnter;
            equipmentSlots[i].OnExit += OnExit;
        }
    }

    private void OnValidate()
    {
        equipmentSlots = equipmentSlotsParent.GetComponentsInChildren<EquipmentSlot>();
    }

    public bool AddItem(EquippableItem item, out EquippableItem previousItem)
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if (equipmentSlots[i].EquipmentType == item.EquipmentType)
            {
                previousItem = (EquippableItem)equipmentSlots[i].item;
                equipmentSlots[i].item = item;
                UpdateStats();
                return true;
            }
            
        }
        previousItem = null;
        return false;
    }

    public bool RemoveItem(EquippableItem item)
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if (equipmentSlots[i].item == item)
            {
                equipmentSlots[i].item = null;
                UpdateStats();
                return true;
            }
        }
        return false;
    }

    public void UpdateStats()
    {
        int intAttack = (int)player.baseAttack;
        int intAgility = 0;
        int intDefense = 0;

        foreach (EquipmentSlot es in equipmentSlots)
        {
            EquippableItem ei = (EquippableItem)es.item;

            if (ei)
            {
                intAttack += ei.AttackBonus;
                intAgility += ei.AgillityBonus;
                intDefense += ei.DefenseBonus;
            }
        }
        attack.ValueText.text = intAttack.ToString();
        agility.ValueText.text = intAgility.ToString();
        defense.ValueText.text = intDefense.ToString();
        player.UpdateVars(intDefense, intAttack, intAgility);
    }
}

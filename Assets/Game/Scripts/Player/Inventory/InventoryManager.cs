using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    [SerializeField] EquipmentPanel equipmentPanel;
    [SerializeField] Inventory chestInv;
    [SerializeField] Inventory actualChest;

    private void Awake()
    {
        inventory.OnItemRightClickedEvent += EquipFromInventory;
        equipmentPanel.OnItemRightClickedEvent += UnequipFromEquipPanel;
    }

    public void FetchChestInv(Inventory visual, Inventory actual)
    {
        chestInv = visual;
        actualChest = actual;
        chestInv.OnItemRightClickedEvent += MoveFrom;
        chestInv.AddAction();
        Debug.Log("from set");
    }

    private void EquipFromInventory(Item item)
    {
        if (equipmentPanel.transform.parent.gameObject.activeSelf)
        {
            if (item is EquippableItem)
            {
                Equip((EquippableItem)item);
            }
        }
        else if (chestInv.gameObject.activeSelf)
        {
            MoveTo(item);
        }
    }

    private void UnequipFromEquipPanel(Item item)
    {
        if (item is EquippableItem)
        {
            Unequip((EquippableItem)item);
        }
    }

    public void Equip(EquippableItem item)
    {
        if (inventory.RemoveItem(item))
        {
            EquippableItem previousItem;
            if (equipmentPanel.AddItem(item, out previousItem))
            {
                if (previousItem != null)
                {
                    inventory.AddItem(previousItem);
                }
            }
            else
            {
                inventory.AddItem(item);
            }
        }
    }

    private void MoveTo(Item item)
    {
        if (inventory.RemoveItem(item))
        {
            if (!AddToChest(item))
            {
                inventory.AddItem(item);
            }
        }
    }

    private void MoveFrom(Item item)
    {
        Debug.Log("From registered");
        if (!inventory.IsFull() && RemoveFromChest(item))
        {
            inventory.AddItem(item);
        }
    }

    private bool AddToChest(Item item)
    {
        return chestInv.AddItem(item) | actualChest.AddItem(item);
    }

    private bool RemoveFromChest(Item item)
    {
        return chestInv.RemoveItem(item) | actualChest.RemoveItem(item);
    }

    public void Unequip(EquippableItem item)
    {
        if (!inventory.IsFull() && equipmentPanel.RemoveItem(item))
        {
            inventory.AddItem(item);
        }
    }
}

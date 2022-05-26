using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInventory", menuName = "Item System/Add Inventory", order = 1)]
public class InventoryObject : ScriptableObject
{
    public int maxInventorySize = 30;

    public List<ItemSlot> itemSlots = new List<ItemSlot>();

    public void AddItem(ItemObject itemObject, int quantity)
    {
        for (int i = 0; i < itemSlots.Count; i++)
        {
            if (itemSlots[i].itemObject == itemObject && itemSlots[i].stackQuantity < itemObject.maxStackQuantity)
            {
                quantity = itemSlots[i].AddToStack(quantity);
                if (quantity <= 0)
                {
                    break;
                }
            }
        }

        if (quantity > 0 && itemSlots.Count < maxInventorySize)
        {
            itemSlots.Add(new ItemSlot(itemObject, quantity));
        }
    }

    public void RemoveItem(ItemObject io, int quantity)
    {
        for (int i = 0; i < itemSlots.Count; i++)
        {
            if (itemSlots[i].itemObject == io)
            {
                if (itemSlots[i].RemoveFromStack(quantity) == 0)
                {
                    itemSlots.RemoveAt(i);
                }
            }
        }
    }

    public void ClearInventory()
    {
        itemSlots = new List<ItemSlot>();
    }
}
[System.Serializable]
public class ItemSlot
{
    public ItemObject itemObject;
    public int stackQuantity;

    public ItemSlot(ItemObject itemObject, int stackQuantity)
    {
        this.itemObject = itemObject;
        this.stackQuantity = stackQuantity;
    }

    public int AddToStack(int quantity)
    {
        stackQuantity += quantity;
        int remainingQuantity = stackQuantity - itemObject.maxStackQuantity;

        if (remainingQuantity < 0)
        {
            remainingQuantity = 0;
        }

        stackQuantity -= remainingQuantity;
        return remainingQuantity;
    }

    public int RemoveFromStack(int quantity)
    {
        if (stackQuantity - quantity < 0)
        {
            quantity = stackQuantity;
        }

        stackQuantity -= quantity;

        return stackQuantity;
    }
}
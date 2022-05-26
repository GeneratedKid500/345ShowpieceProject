using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    public InventoryObject inventory;
    DisplayInventory di;
    ThirdPersonControl fpc;

    public string InventoryButton1;
    public string InventoryButton2;

    int itemIndex;

    private void Start()
    {
        fpc = GetComponent<ThirdPersonControl>();
        di = GameObject.FindGameObjectWithTag("MainCanvas").GetComponentInChildren<DisplayInventory>();   
    }

    private void Update()
    {
        if (!pauseMenu.activeInHierarchy)
        {
            if (Input.GetButtonDown(InventoryButton1) || Input.GetButtonDown(InventoryButton2))
            {
                if (di.OpenInventory())
                {
                    fpc.enabled = false;
                    Time.timeScale = 0;
                }
                else
                {
                    fpc.enabled = true;
                    Time.timeScale = 1;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Item item = other.GetComponentInParent<Item>();
        if (item != null)
        {
            inventory.AddItem(item.item, 1);
            Destroy(other.gameObject);
        }
    }

    public void RecieveItemIndex(int index)
    {
        itemIndex = index;
    }

    public void RemoveItem(int amount = -1)
    {
        if (amount == -1)
        {
            amount = inventory.itemSlots[itemIndex].stackQuantity;
        }
        Debug.Log(amount);
        for (int i = 0; i < amount; i++)
        {
            Instantiate(inventory.itemSlots[itemIndex].itemObject.itemPrefab, new Vector3(transform.position.x + 2, transform.position.y, transform.position.z + 2), Quaternion.identity);
        }
        inventory.RemoveItem(inventory.itemSlots[itemIndex].itemObject, amount);
    }

    public void RemoveOne()
    {
        inventory.RemoveItem(inventory.itemSlots[itemIndex].itemObject, 1);
    }
}

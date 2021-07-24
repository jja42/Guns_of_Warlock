using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> characterItems = new List<Item>();
    public ItemDatabase itemDatabase;
    public UI_Inventory inventoryUI;
    bool init = false;
    public void Start()
    {
        inventoryUI.gameObject.SetActive(false);
    }
    public void Update()
    {
        if (!init)
        {
            GiveItem("Gun");
            GiveItem("Fireball");
            init = true;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryUI.gameObject.SetActive(!inventoryUI.gameObject.activeSelf);
        }
    }
    public void GiveItem(string itemName)
    {
        Item itemToAdd = itemDatabase.GetItem(itemName);
        characterItems.Add(itemToAdd);
        inventoryUI.gameObject.SetActive(true);
        inventoryUI.AddNewItem(itemToAdd);
        inventoryUI.gameObject.SetActive(false);
    }
    public Item CheckForItem(string itemName)
    {
        return characterItems.Find(item => item.title == itemName);
    }
    public void RemoveItem(string itemName)
    {
        Item item = CheckForItem(itemName);
        if (item != null)
        {
            characterItems.Remove(item);
            inventoryUI.RemoveItem(item);
        }
    }
}

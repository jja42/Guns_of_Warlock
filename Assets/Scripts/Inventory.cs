using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public List<Item> characterItems = new List<Item>();
    public UI_Inventory inventoryUI;
    bool init;
    private void Awake()
    {
        instance = this;
    }
    public void Start()
    {
        inventoryUI.gameObject.SetActive(false);
        init = false;
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
        Item itemToAdd = new Item(ItemDatabase.instance.GetItem(itemName));
        itemToAdd.owner = 0;
        characterItems.Add(itemToAdd);
        bool state = inventoryUI.gameObject.activeSelf;
        inventoryUI.gameObject.SetActive(true);
        inventoryUI.AddNewItem(itemToAdd);
        inventoryUI.gameObject.SetActive(state);
    }
    public Item CheckForItem(string itemName)
    {
        return characterItems.Find(item => item.name == itemName);
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

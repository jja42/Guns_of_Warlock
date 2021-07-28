using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public List<UI_Item> UI_Items = new List<UI_Item>();
    public GameObject slotPrefab;
    public Transform slotPanel;
    public Text Item_Tooltip;
    public int numberOfSlots;
    public GameObject ShopUI;
    private void Awake()
    {
        for (int i = 0; i < numberOfSlots; i++)
        {
            GameObject obj = Instantiate(slotPrefab);
            obj.transform.SetParent(slotPanel);
            UI_Items.Add(obj.GetComponentInChildren<UI_Item>());
        }
    }
    private void Start()
    {
        GiveItem("Invisibility Potion");
        GiveItem("Small Health Potion");
    }
    public void GiveItem(string itemName)
    {
        Item itemToAdd = ItemDatabase.instance.GetItem(itemName);
        ShopUI.SetActive(true);
        AddNewItem(itemToAdd);
        ShopUI.SetActive(false);
    }
    public void UpdateSlot(int slot, Item item)
    {
        UI_Items[slot].UpdateItem(item);
    }
    public void AddNewItem(Item item)
    {
        UpdateSlot(UI_Items.FindIndex(i => i == null || i.item == null), item);
    }
    public void RemoveItem(Item item)
    {
        UpdateSlot(UI_Items.FindIndex(i => i == null || i.item == item), null);
    }
    public void ToggleShop()
    {
        ShopUI.SetActive(!ShopUI.activeSelf);
    }
}

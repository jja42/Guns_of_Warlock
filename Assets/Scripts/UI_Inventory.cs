using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    public List<UI_Item> UI_Items = new List<UI_Item>();
    public GameObject slotPrefab;
    public Transform slotPanel;
    public int numberOfSlots;
    private void Awake()
    {
        for (int i = 0; i < numberOfSlots; i++)
        {
            GameObject instance = Instantiate(slotPrefab);
            instance.transform.SetParent(slotPanel);
            UI_Items.Add(instance.GetComponentInChildren<UI_Item>());
            UI_Items[i].index = i;
        }
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
    public void RemoveItem(int index)
    {
        UpdateSlot(index, null);
    }
}

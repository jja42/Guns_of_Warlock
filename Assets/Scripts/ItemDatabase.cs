using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public void Awake()
    {
        BuildDatabase();
    }

    public Item GetItem(string itemName)
    {
        return items.Find(item => item.title == itemName);
    }
    void BuildDatabase()
    {
        items = new List<Item>() {
            new Item(0, "Gun", "A warlock's best friend and a powerful magical conduit."),
            new Item(1, "Fireball", "Your standard spell. Comes out quick, does some damage."),
        };
    }
}

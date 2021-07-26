using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase instance;
    public List<Item> items = new List<Item>();
    public void Awake()
    {
        BuildDatabase();
        instance = this;
    }

    public Item GetItem(string itemName)
    {
        return items.Find(item => item.name == itemName);
    }
    void BuildDatabase()
    {
        items = new List<Item>() {
            new Item(0, "Gun", "A warlock's best friend and a powerful magical conduit.",0,0),
            new Item(1, "Fireball", "Your standard spell. Comes out quick, does some damage.",0,0),
            new Item(2,"Invisibility Potion","Don't do nothing pervy with it.",100,1)
        };
    }
}
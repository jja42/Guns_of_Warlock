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
            new Item(0, "Gun", "A warlock's best friend and a powerful magical conduit.",0,0,1,1),
            new Item(1, "Acid Shot", "A low level spell. Comes out quick, does some damage.",0,0,1,1),
            new Item(2,"Invisibility Potion","Don't do nothing pervy with it.",200,1,0,2),
            new Item(3, "Small Health Potion","Restores one Heart. Does ya good.",20,1,0,2),
            new Item(4,"Shotgun","A powerful firearm, suitable only for the best warlocks.",240,1,1,1),
            new Item(5, "Water Bolt", "A mid level spell. This'll leave a mark.",120,1,1,1),
            new Item(6, "Fireball", "A high level spell. It packs some heat. Experienced Warlocks Only.",120,1,1,1),
            new Item(7, "Large Health Potion","Restores two Hearts. Part of a balanced breakfast.",20,1,0,2)
        };
    }
}
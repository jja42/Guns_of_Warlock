using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public int id;
    public string name;
    public string description;
    public Sprite icon;
    public int cost;
    public int owner;
    public int count;
    public int max_count;
    public Item(int item_id, string item_name, string item_description, int item_cost, int item_owner, int item_count, int item_max_count)
    {
        id = item_id;
        name = item_name;
        description = item_description;
        icon = Resources.Load<Sprite>("Items/" + name);
        cost = item_cost;
        owner = item_owner;
        count = item_count;
        max_count = item_max_count;
    }
    public Item(Item item)
    {
        id = item.id;
        name = item.name;
        description = item.description;
        icon = Resources.Load<Sprite>("Items/" + item.name);
        cost = item.cost;
        owner = item.owner;
        count = item.count;
        max_count = item.max_count;
    }
}
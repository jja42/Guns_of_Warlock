using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public int id;
    public string title;
    public string description;
    public Sprite icon;
    public Item(int item_id, string item_title, string item_description)
    {
        id = item_id;
        title = item_title;
        description = item_description;
        icon = Resources.Load<Sprite>("Items/" + title);
    }
    public Item(Item item)
    {
        id = item.id;
        title = item.title;
        description = item.description;
        icon = Resources.Load<Sprite>("Items/" + item.title);
    }
}
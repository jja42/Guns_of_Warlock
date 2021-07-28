using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Item : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler, IPointerClickHandler
{
    public Item item;
    public int index;
    private Image spriteImage;
    Text Item_Tooltip;
    //private UI_Item selectedItem;
    private void Awake()
    {
        spriteImage = GetComponent<Image>();
        UpdateItem(null);
        Item_Tooltip = UI_Manager.instance.Item_Tooltip;
        //selectedItem = GameObject.Find("SelectedItem").GetComponent<UI_Item>();
    }

    public void UpdateItem(Item Item)
    {
        item = Item;
        if (item != null)
        {
            spriteImage.color = Color.white;
            spriteImage.sprite = item.icon;
        }
        else
        {
            spriteImage.color = Color.clear;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (item != null)
        {
            if (item.owner == 1)
            {
                if (Game_Manager.instance.money >= item.cost)
                {
                    Game_Manager.instance.money -= item.cost;
                    if (Inventory.instance.CheckStackable(item.name) != null)
                    {
                        int count = Inventory.instance.StackItem(item.name);
                    }
                    else
                    {
                        Inventory.instance.GiveItem(item.name);
                    }
                    if (item.count == 1)
                    {
                        Game_Manager.instance.RemoveShopItem(item);
                    }
                    Inventory.instance.TradeSound();
                }
            }
            else
            {
                if (Game_Manager.instance.shopping && item.cost > 0)
                {
                    Game_Manager.instance.money += item.cost / 2;
                    Inventory.instance.RemoveItem(item.name,index);
                    Inventory.instance.TradeSound();
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
        {
            Item_Tooltip.enabled = true;
            Item_Tooltip.rectTransform.position = transform.position;
            Item_Tooltip.rectTransform.position = new Vector3(Item_Tooltip.rectTransform.position.x, Item_Tooltip.rectTransform.position.y - 100,Item_Tooltip.rectTransform.position.z);
            Item_Tooltip.text = item.name + " ";
            if (item.cost > 0 && Game_Manager.instance.shopping)
            {
                if (item.owner == 1)
                {
                    Item_Tooltip.text += "(" + item.cost + "g)";
                }
                else
                {
                    Item_Tooltip.text += "(" + item.cost / 2 + "g)";
                }
            }
            Item_Tooltip.text += "\n";
            Item_Tooltip.text += item.description;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Item_Tooltip.enabled = false;
    }
}
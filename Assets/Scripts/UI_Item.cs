using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Item : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler, IPointerClickHandler
{
    public Item item;
    private Image spriteImage;
    Text Item_Tooltip;
    //private UI_Item selectedItem;
    private void Awake()
    {
        spriteImage = GetComponent<Image>();
        UpdateItem(null);
        //selectedItem = GameObject.Find("SelectedItem").GetComponent<UI_Item>();
    }
    private void Start()
    {
        Item_Tooltip = Game_Manager.instance.Item_Tooltip;   
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
                    Inventory.instance.GiveItem(item.name);
                }
            }
            else
            {
                if (Game_Manager.instance.shopping && item.cost > 0)
                {
                    Game_Manager.instance.money += item.cost / 2;
                    Inventory.instance.RemoveItem(item.name);
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
            Item_Tooltip.rectTransform.position = new Vector3(Item_Tooltip.rectTransform.position.x, Item_Tooltip.rectTransform.position.y - 70,Item_Tooltip.rectTransform.position.z);
            Item_Tooltip.text = item.name + "\n";
            Item_Tooltip.text += item.description;
            if(item.cost > 0)
            {
                if (item.owner == 1)
                {
                    Item_Tooltip.text += "\n Cost: " + item.cost;
                }
                else
                {
                    Item_Tooltip.text += "\n Value: " + item.cost/2;
                }
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Item_Tooltip.enabled = false;
    }
}
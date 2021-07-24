using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Item : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
    public Item item;
    private Image spriteImage;
    //private UI_Item selectedItem;
    private void Awake()
    {
        spriteImage = GetComponent<Image>();
        UpdateItem(null);
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

    /*public void OnPointerClick(PointerEventData eventData)
    {
        if (item != null)
        {
            if (selectedItem.item != null)
            {
                Item clone = new Item(selectedItem.item);
                selectedItem.UpdateItem(item);
                UpdateItem(clone);
            }
            else
            {
                selectedItem.UpdateItem(item);
                UpdateItem(null);
            }
        }
        else if (selectedItem.item != null)
        {
            UpdateItem(selectedItem.item);
            selectedItem.UpdateItem(null);
        }
    }*/

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
        {
            UI_Inventory.instance.Item_Tooltip.enabled = true;
            UI_Inventory.instance.Item_Tooltip.rectTransform.position = transform.position;
            UI_Inventory.instance.Item_Tooltip.rectTransform.position = new Vector3(UI_Inventory.instance.Item_Tooltip.rectTransform.position.x, UI_Inventory.instance.Item_Tooltip.rectTransform.position.y - 70,UI_Inventory.instance.Item_Tooltip.rectTransform.position.z);
            UI_Inventory.instance.Item_Tooltip.text = item.description;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UI_Inventory.instance.Item_Tooltip.enabled = false;
    }
}
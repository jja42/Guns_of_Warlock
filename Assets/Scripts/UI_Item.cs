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
    public Text stack_text;
    //private UI_Item selectedItem;
    private void Awake()
    {
        spriteImage = GetComponent<Image>();
        UpdateItem(null);
        //selectedItem = GameObject.Find("SelectedItem").GetComponent<UI_Item>();
    }

    public void Start()
    {
        Item_Tooltip = UI_Manager.instance.Item_Tooltip;
    }
    void Update()
    {
        if (item != null)
        {
            if (item.count > 1)
            {
                stack_text.gameObject.SetActive(true);
                stack_text.text = item.count.ToString();
            }
            else
            {
                stack_text.gameObject.SetActive(false);
            }
        }
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
                if (Game_Manager.instance.money >= item.cost && Inventory.instance.characterItems.Count < 8)
                {
                    Game_Manager.instance.money -= item.cost;
                    if (Inventory.instance.CheckStackable(item.name) != null)
                    {
                        Inventory.instance.StackItem(item.name);
                    }
                    else
                    {
                        if (item.name.Equals("Shotgun"))
                        {
                            Data_Manager.instance.Flags[5] = true;
                            Inventory.instance.ReplaceItem(item.name, 0);
                        }
                        if (item.name.Equals("Water Bolt"))
                        {
                            Data_Manager.instance.Flags[4] = true;
                            Inventory.instance.ReplaceItem(item.name, 1);
                        }
                        if (item.name.Equals("Fireball"))
                        {
                            Data_Manager.instance.Flags[8] = true;
                            Inventory.instance.ReplaceItem(item.name, 1);
                        }
                        if(item.name.Equals("Ring of Game Mechanic Progression"))
                        {
                            Data_Manager.instance.Flags[9] = true;
                            Inventory.instance.GiveItem(item.name,true);
                        }
                        else
                        {
                            Inventory.instance.GiveItem(item.name,false);
                        }
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
                else
                {
                    if (item.name.Equals("Small Health Potion"))
                    {
                        Game_Manager.instance.player_health += 1;
                        Inventory.instance.SlurpSound();
                        Inventory.instance.RemoveItem(item.name, index);
                    }
                    else
                    {
                        if (item.name.Equals("Large Health Potion"))
                        {
                            Game_Manager.instance.player_health += 2;
                            Inventory.instance.SlurpSound();
                            Inventory.instance.RemoveItem(item.name, index);
                        }
                        else
                        {
                            if (item.name.Equals("Invisibility Potion"))
                            {
                                Game_Manager.instance.invisible = true;
                                Inventory.instance.SlurpSound();
                                Inventory.instance.RemoveItem(item.name, index);
                            }
                        }
                    }
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
            Item_Tooltip.rectTransform.position = new Vector3(Item_Tooltip.rectTransform.position.x, Item_Tooltip.rectTransform.position.y - 100);
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
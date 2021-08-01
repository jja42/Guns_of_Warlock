using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public List<Item> characterItems = new List<Item>();
    UI_Inventory inventoryUI;
    bool init;
    AudioSource audioSource;
    public AudioClip coin_sfx;
    public AudioClip slurp_sfx;
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this);
    }
    public void Start()
    {
        inventoryUI = FindObjectOfType<UI_Inventory>();
        inventoryUI.gameObject.SetActive(false);
        init = false;
        audioSource = GetComponent<AudioSource>();
    }
    public void Update()
    {
        if(inventoryUI == null)
        {
            inventoryUI = FindObjectOfType<UI_Inventory>();
            inventoryUI.gameObject.SetActive(false);
            ReloadInventory();
        }
        if (!init)
        {
            GiveItem("Gun",true);
            GiveItem("Acid Shot",true);
            init = true;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryUI.gameObject.SetActive(!inventoryUI.gameObject.activeSelf);
        }
    }
    public void GiveItem(string itemName, bool iskeyitem)
    {
        Item itemToAdd = new Item(ItemDatabase.instance.GetItem(itemName));
        itemToAdd.owner = 0;
        itemToAdd.count = 1;
        if (iskeyitem)
        {
            itemToAdd.cost = 0;
        }
        characterItems.Add(itemToAdd);
        bool state = inventoryUI.gameObject.activeSelf;
        inventoryUI.gameObject.SetActive(true);
        inventoryUI.AddNewItem(itemToAdd);
        inventoryUI.gameObject.SetActive(state);
    }
    public void ReplaceItem(string itemName, int index)
    {
        Item itemToAdd = new Item(ItemDatabase.instance.GetItem(itemName));
        itemToAdd.owner = 0;
        itemToAdd.count = 1;
        itemToAdd.cost = 0;
        characterItems[index] = itemToAdd;
        bool state = inventoryUI.gameObject.activeSelf;
        inventoryUI.gameObject.SetActive(true);
        inventoryUI.UpdateSlot(index,itemToAdd);
        inventoryUI.gameObject.SetActive(state);
    }
    public int StackItem(string itemName)
    {
        Item item = characterItems.Find(item => item.name == itemName && item.count < item.max_count);
        item.count++;
        return item.count;
    }
    public Item CheckForItem(string itemName)
    {
        return characterItems.Find(item => item.name == itemName);
    }
    public Item CheckStackable(string itemName)
    {
        return characterItems.Find(item => item.name == itemName && item.count < item.max_count);
    }
    public void RemoveItem(string itemName, int index)
    {
        Item item = CheckForItem(itemName);
        if (item != null)
        {
                item.count -= 1;
                characterItems.Remove(item);
                inventoryUI.RemoveItem(index);
        }
    }
    public void RemoveItem(string itemName)
    {
        Item item = CheckForItem(itemName);
        if (item != null)
        {
            characterItems.Remove(item);
            inventoryUI.RemoveItem(item);
        }
    }
    public void TradeSound()
    {
        audioSource.PlayOneShot(coin_sfx);
    }
    public void SlurpSound()
    {
        audioSource.PlayOneShot(slurp_sfx);
    }
    void ReloadInventory()
    {
        foreach( Item item in characterItems){
            bool state = inventoryUI.gameObject.activeSelf;
            inventoryUI.gameObject.SetActive(true);
            inventoryUI.AddNewItem(item);
            inventoryUI.gameObject.SetActive(state);
        }
    }
}

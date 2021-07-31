using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager instance;
    public bool paused = false;
    public int money;
    public bool shopping;
    public int player_health;
    int max_player_health;
    public bool double_jump;
    public int player_lives;
    public bool Greg;
    Shop shop;
    PlayerCont player;
    // Start is called before the first frame update
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
    private void Start()
    {
        player_health = 3;
        player_lives = 3;
        max_player_health = player_health;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
        }
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("ShopkeeperHouse") && shop == null)
        {
            shop = FindObjectOfType<Shop>();
        }
        if(player == null)
        {
            player = FindObjectOfType<PlayerCont>();
        }
        UI_Manager.instance.money_text.text = money.ToString();
        UI_Manager.instance.health.fillAmount = (float)player_health / max_player_health;
        UI_Manager.instance.player_lives.text = player_lives.ToString();
    }

    public void LoadScene(int scene_index)
    {
        SceneManager.LoadScene(scene_index);
    }

    public void RemoveShopItem(Item item)
    {
        shop.RemoveItem(item);
    }
    public void ToggleShop()
    {
        shop.ToggleShop();
        shopping = !shopping;
    }
    public void KillPlayer()
    {
        player.StartCoroutine(player.Respawn());
    }
    public void ActivatePopup()
    {
        StartCoroutine(UI_Manager.instance.ActivatePopup());
    }
}

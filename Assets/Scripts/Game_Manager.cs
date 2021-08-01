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
    public int player_lives;
    public bool Greg;
    Shop shop;
    PlayerCont player;
    GameObject Greg_NPC;
    float invisible_timer = 5;
    public bool invisible;
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
        if (invisible)
        {
            invisible_timer -= Time.deltaTime;
        }
        else
        {
            invisible_timer = 5;
        }
        if(invisible_timer<= 0)
        {
            invisible = false;
        }
        player_health = Mathf.Clamp(player_health,0, 3);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
        }
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("ShopkeeperHouse"))
        {
            if(shop == null)
                shop = FindObjectOfType<Shop>();
        }
        else
        {
            shopping = false;
        }
        if(player == null)
        {
            player = FindObjectOfType<PlayerCont>();
        }
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("GregShack")){
            Greg_NPC = GameObject.FindGameObjectWithTag("Greg");
            if (Data_Manager.instance.Flags[3])
            {
                Greg_NPC.name = "Greg2";
            }
        }
        UI_Manager.instance.money_text.text = money.ToString();
        UI_Manager.instance.health.fillAmount = (float)player_health / max_player_health;
        UI_Manager.instance.player_lives.text = player_lives.ToString();
    }

    public void LoadScene(int scene_index)
    {
        paused = true;
        SceneManager.LoadScene(scene_index);
        paused = false;
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
        Data_Manager.instance.Flags[3] = true;
    }
    public void ActivatePopup()
    {
        StartCoroutine(UI_Manager.instance.ActivatePopup());
    }
    public bool CanGetPosition()
    {
        print(SceneManager.GetActiveScene().buildIndex);
        if (Data_Manager.instance.Positions[SceneManager.GetActiveScene().buildIndex] != Vector3.zero)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public Vector3 GetPosition()
    {
            return Data_Manager.instance.Positions[SceneManager.GetActiveScene().buildIndex];
    }
    public void SetSpawn()
    {
        if(player != null)
            Data_Manager.instance.Positions[SceneManager.GetActiveScene().buildIndex] = player.transform.position;
    }
}

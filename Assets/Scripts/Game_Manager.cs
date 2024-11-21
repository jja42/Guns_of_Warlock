using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
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
    public PlayerInput input;
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
        // Limit the framerate to 30
        Application.targetFrameRate = 30;
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
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("ShopkeeperHouse"))
        {
            if(shop == null)
                shop = FindFirstObjectByType<Shop>();
        }
        else
        {
            shopping = false;
        }
        if(player == null)
        {
            player = FindFirstObjectByType<PlayerCont>();
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
        UI_Manager.instance.ToggleShop();
    }
    public void KillPlayer()
    {
        player.StartCoroutine(player.Respawn());
        Data_Manager.instance.Flags[3] = true;
    }
    public void ActivatePopup()
    {
        StartCoroutine(UI_Manager.instance.ActivatePopup());
        if (Data_Manager.instance.Flags[2])
        {
            StartCoroutine(FadetoBlack());
        }
    }

    public bool CanGetPosition()
    {
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
    IEnumerator FadetoBlack()
    {
        UI_Manager.instance.FadeBlack();
        Inventory.instance.Victory();
        yield return new WaitForSeconds(5);
        LoadScene(11);
        Destroy(gameObject);
    }

    void OnMove(InputValue value)
    {
        Vector2 inputvector = value.Get<Vector2>();
        inputvector = new Vector2(inputvector.x, 0);
        inputvector = inputvector.normalized;
        player.PlayerMove(inputvector);
    }

    void OnJump()
    {
        player.PlayerJump();
    }

    void OnShoot()
    {
        player.PlayerShoot();
    }

    public void Respawn()
    {
        player.PlayerRespawn();
    }

    void OnInteract()
    {
        player.PlayerInteract();
    }

    void OnPause()
    {
        if (!paused)
        {
            paused = true;
            UIFocus();
            UI_Manager.instance.PauseMenu();
            Menu_Manager.instance.ChangeLayer(1);
        }
    }

    public void GameplayFocus()
    {
        input.currentActionMap.Disable();
        input.SwitchCurrentActionMap("Gameplay");
        input.currentActionMap.Enable();
    }

    public void UIFocus()
    {
        input.currentActionMap.Disable();
        input.SwitchCurrentActionMap("UI");
        input.currentActionMap.Enable();
    }

    void OnNavigate()
    {
        if (input.currentActionMap.name == "UI")
        {
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                UI_Item item = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<UI_Item>();
                if (item != null)
                {
                    item.SelectItem();
                }
            }
        }
    }
}

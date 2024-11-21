using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager instance;
    public Text Item_Tooltip;
    public Text money_text;
    public Image health;
    public Text player_lives;
    public GameObject inventory;
    public Text Popup;
    public Image Fade;
    public GameObject Pause;
    float fade_float = 0;
    bool fading;
    public GameObject ShopUI;
    public Transform ShopPanel;
    // Start is called before the first frame update
    private void Awake()
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
        inventory.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        if (fading)
        {
            Fade.color = new Color(0, 0, 0, fade_float);
            fade_float += Time.deltaTime / 3;
        }
    }
    public IEnumerator ActivatePopup()
    {
        Popup.gameObject.SetActive(true);
        yield return new WaitForSeconds(5);
        Popup.gameObject.SetActive(false);
    }
    public void FadeBlack()
    {
        fading = true;
    }

    public void PauseMenu()
    {
        Pause.gameObject.SetActive(true);
    }

    public void Unpause()
    {
        Pause.gameObject.SetActive(false);
        Game_Manager.instance.paused = false;
        Menu_Manager.instance.ChangeLayer(0);
        Game_Manager.instance.GameplayFocus();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void CloseInventory()
    {
        Inventory.instance.CloseInventory();
    }

    public void ToggleShop()
    {
        ShopUI.SetActive(!ShopUI.activeSelf);
        if (ShopUI.gameObject.activeSelf)
        {
            Menu_Manager.instance.ChangeLayer(1);
            Game_Manager.instance.UIFocus();
            Game_Manager.instance.shopping = true;
        }
        else
        {
            Game_Manager.instance.shopping = false;
            Menu_Manager.instance.ChangeLayer(0);
            Game_Manager.instance.GameplayFocus();
        }
    }

    public void Respawn()
    {
        Unpause();
        Game_Manager.instance.Respawn();
    }
}

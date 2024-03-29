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
    public Image Inventory;
    public Text Popup;
    public Image Fade;
    float fade_float = 0;
    bool fading;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        Inventory.color = new Color(Inventory.color.r, Inventory.color.g, Inventory.color.b);
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
}

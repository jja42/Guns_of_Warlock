using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{
    public GameObject Instructions;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void MenuStart()
    {
        SceneManager.LoadScene(1);
    }
    public void MenuExit()
    {
        Application.Quit();
    }
    public void InstructionsPanel()
    {
        Instructions.SetActive(!Instructions.activeSelf);
    }
}

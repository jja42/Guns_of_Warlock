using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Menu_Manager : MonoBehaviour
{
    //References for each layer
    public List<GameObject> MenuLayers;
    public int FirstActiveLayer;

    //Variable to hold our active layer
    GameObject activeLayer;

    //List of the options that should be active
    List<GameObject> ActiveMenuOptions;

    //Static Reference to use for calls from other scripts
    public static Menu_Manager instance;

    private void Awake()
    {
        //Standard Singleton Setup
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        //Initialize List
        ActiveMenuOptions = new List<GameObject>();

        ChangeLayer(FirstActiveLayer);
    }

    //Change our active layer based on int passed
    //Get menu options based on new active layer
    //Enable all interactible ui
    //Assign Selected GameObject for Event System
    //Disable Unintentional Interaction with other layers
    public void ChangeLayer(int layer_num)
    {
        activeLayer = MenuLayers[layer_num];
        ActiveMenuOptions.Clear();
        ActiveMenuOptions = GetMenuOptions(activeLayer);
        ToggleMenuOptions(ActiveMenuOptions, true);
        ActiveMenuOptions = FilterMenuOptions(ActiveMenuOptions);

        if (ActiveMenuOptions.Count > 0)
        {
            EventSystem.current.SetSelectedGameObject(ActiveMenuOptions[0]);
            //print(EventSystem.current.currentSelectedGameObject.name);
        }

        foreach (GameObject layer in MenuLayers)
        {
            if (layer != activeLayer)
            {
                ToggleMenuOptions(GetMenuOptions(layer), false);
            }
        }
    }

    //Find all children of the layer
    //Gets Gameobject children that have selectable ui on them
    List<GameObject> GetMenuOptions(GameObject layer)
    {
        List<GameObject> options = new List<GameObject>();

        List<GameObject> objects = FindAllChildren(layer);

        foreach (GameObject gameObject in objects)
        {
            if (gameObject.GetComponent<Selectable>() != null)
            {
                options.Add(gameObject);
                //print(layer.name + ": " + gameObject.name);
            }
        }
        return options;
    }

    //Disables interactibility for non-active layers
    //Enables interactibility for active layer
    void ToggleMenuOptions(List<GameObject> options, bool active)
    {
        if (active)
        {
            foreach (GameObject option in options)
            {
                Selectable ui_element = option.GetComponent<Selectable>();
                ui_element.interactable = true;
            }
        }
        else
        {
            foreach (GameObject option in options)
            {
                Selectable ui_element = option.GetComponent<Selectable>();
                ui_element.interactable = false;
            }
        }
    }

    //Filter our interactible menu options to only those that are active
    List<GameObject> FilterMenuOptions(List<GameObject> options)
    {
        List<GameObject> filtered_options = new List<GameObject>();
        foreach (GameObject option in options)
        {
            if (option.activeInHierarchy)
            {
                filtered_options.Add(option);
            }
        }
        return filtered_options;
    }

    List<GameObject> FindAllChildren(GameObject GO)
    {
        List<GameObject> objects = new List<GameObject>();
        foreach (Transform transform in GO.GetComponentInChildren<Transform>())
        {
            objects.Add(transform.gameObject);
            if (transform.childCount > 0)
            {
                objects.AddRange(FindAllChildren(transform.gameObject));
            }
        }
        return objects;

    }

}
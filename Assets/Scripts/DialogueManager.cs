using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    void Awake()
    {
        instance = this;
    }
    private Story story;
    public Text textPrefab;
    public Button buttonPrefab;
    public bool talking = false;
    public bool more_dialogue = false;
    // Start is called before the first frame update
    public void PlayDialogue(string name)
    {
        talking = true;
        string title = name;
        TextAsset inkJSON = Resources.Load<TextAsset>("Ink/" + title);
        story = new Story(inkJSON.text);
        story.BindExternalFunction("set_flag", (int flag) =>
        {
            Data_Manager.instance.data.flags[flag] = 1;
            return 0;
        });
        story.BindExternalFunction("check_item", (string item) =>
        {

            if (Inventory.instance.CheckForItem(item) != null)
            {
                return 1;
            }
            return 0;
        });
        story.BindExternalFunction("magic_trick",()=>
            {
                Game_Manager.instance.Greg = true;
                Game_Manager.instance.KillPlayer();
        });
        refreshUI();
    }

    void refreshUI()
    {
        eraseUI();

        Text storyText = Instantiate(textPrefab);
        storyText.text = loadStoryChunk();
        storyText.transform.SetParent(transform, false);

        foreach (Choice choice in story.currentChoices)
        {
            Button choiceButton = Instantiate(buttonPrefab);
            choiceButton.transform.SetParent(transform, false);

            // Gets the text from the button prefab
            Text choiceText = choiceButton.GetComponentInChildren<Text>();
            choiceText.text = choice.text;

            // Set listener
            choiceButton.onClick.AddListener(delegate
            {
                chooseStoryChoice(choice);
            });
        }
        if (story.currentChoices.Count == 0)
        {
            more_dialogue = false;
            Button choiceButton = Instantiate(buttonPrefab) as Button;
            choiceButton.transform.SetParent(this.transform, false);
            Text choiceText = choiceButton.GetComponentInChildren<Text>();
            choiceText.text = "Exit";
            choiceButton.onClick.AddListener(delegate
            {
                ExitStory();
            });
        }
        else
        {
            more_dialogue = true;
        }
    }
    public void ExitStory()
    {
        if ((string)story.variablesState["giveitem"] != null)
        {
            string item = (string)story.variablesState["giveitem"];
            Inventory.instance.GiveItem(item,true);
        }
        if ((string)story.variablesState["removeitem"] != null)
        {
            string item = (string)story.variablesState["removeitem"];
            Inventory.instance.RemoveItem(item);
        }
        talking = false;
        eraseUI();
    }

    void eraseUI()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Destroy(this.transform.GetChild(i).gameObject);
        }
    }

    void chooseStoryChoice(Choice choice)
    {
        story.ChooseChoiceIndex(choice.index);
        refreshUI();
    }

    string loadStoryChunk()
    {
        string text = "";
        text = story.ContinueMaximally();
        if (story.canContinue)
        {
            text = story.ContinueMaximally();
        }
        return text;
    }

    public void ContinueStory()
    {
        story.ChooseChoiceIndex(0);
        refreshUI();
    }
}
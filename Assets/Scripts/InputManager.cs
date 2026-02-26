using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using TMPro;
using NUnit.Framework.Constraints;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public TMP_Text storyText; // the story 
    public TMP_InputField userInput; // the input field object
    public TMP_Text inputText; // part of the input field where user enters response
    public TMP_Text placeHolderText; // part of the input field for initial placeholder text
    
    public ScrollRect scrollRect; //Controls how our story scrolls

    private string story; // holds the story to display
    private List<string> commands = new List<string>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        commands.Add("go");
        commands.Add("get");
        commands.Add("restart");
        commands.Add("save");

        story = storyText.text;
        userInput.onEndEdit.AddListener(GetInput);

        //NavagationManager.instance.onRestart += RestartGame;
    }


    IEnumerator ScrollToBottom()
    {
        yield return new WaitForEndOfFrame(); //Wait til after the text is updated

        scrollRect.verticalNormalizedPosition = 0f; //Move to bottom (scroll)
    }



    void GetInput(string input)
    {
        
        userInput.text = "";
        userInput.ActivateInputField();

        if (input != "")
        {
            char[] delims = { ' ' };
            string[] parts = input.ToLower().Split(delims); // parts[0] is the command parts[1] is direction or thing they are picking up

            if (parts.Length >= 2)
            {
                if (commands.Contains(parts[0])) //Valid Command 
                {

                    UpdateStory(input);
                    if (parts[0] == "go")
                    {
                        if (NavagationManager.instance.SwitchRooms(parts[1]))
                            return;
                        else
                            UpdateStory("The exit does not exist or is locked...");
                    }
                    else if (parts[0] == "get")
                    {
                        if (NavagationManager.instance.getItem(parts[1]))
                        {
                            GameManager.instance.inventory.Add(parts[1]);
                        }

                    }
                } //End
            }
            else if (parts.Length == 1)
            {
                if (parts[0] == "restart")
                {
                    NavagationManager.instance.GameRestart();
                }

                else if (parts[0] == "save")
                {
                    GameManager.instance.Save();
                    UpdateStory("Game saved!");
                }
            }

            else //Invalid Command
            {
                UpdateStory("Invalid command. Please try again.");
            }

            }
        }

    public void UpdateStory(string msg)
    {
        story += "\n" + msg;
        storyText.text = story;
        StartCoroutine("ScrollToBottom");
    }
}

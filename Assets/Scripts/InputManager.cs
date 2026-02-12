using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using TMPro;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public TMP_Text storyText; // the story 
    public TMP_InputField userInput; // the input field object
    public TMP_Text inputText; // part of the input field where user enters response
    public TMP_Text placeHolderText; // part of the input field for initial placeholder text
    
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


        story = storyText.text;
        userInput.onEndEdit.AddListener(GetInput);
    }

    void GetInput(string input)
    {
        
        userInput.text = "";
        userInput.ActivateInputField();

        if (input != "")
        {
            char[] delims = { ' ' };
            string[] parts = input.ToLower().Split(delims); // parts[0] is the command parts[1] is direction or thing they are picking up
            
            if(parts.Length > 0 )
            {
                if (commands.Contains(parts[0])) //Valid Command 
                {
                    UpdateStory(input);
                }
                else //Invalid Command
                {
                    UpdateStory("Invalid command. Please try again.");
                }

            }
        }
        
    }

    public void UpdateStory(string msg)
    {
        story += "\n" + msg;
        storyText.text = story;
    }
}

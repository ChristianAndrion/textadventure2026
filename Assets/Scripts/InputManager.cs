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
        commands.Add("commands");
        commands.Add("go");
        commands.Add("get");
        commands.Add("restart");
        commands.Add("save");
        commands.Add("inventory");
        commands.Add("investigate");

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
                            string pickedUpItem;
                            if (parts[1] == "golden")
                                pickedUpItem = "GOLDEN KEY";
                            else if (parts[1] == "blue")
                                pickedUpItem = "BLUE KEY";
                            else if (parts[1] == "red")
                                pickedUpItem = "RED KEY";
                            else
                                pickedUpItem = parts[1].ToUpper();


                            GameManager.instance.inventory.Add(pickedUpItem);
                            
                            UpdateStory("You picked up the " + pickedUpItem);
                        }
                        else
                        {
                            string pickedUpItem;
                            if (parts[1] == "golden")
                                pickedUpItem = "GOLDEN KEY";
                            else if (parts[1] == "blue")
                                pickedUpItem = "BLUE KEY";
                            else if (parts[1] == "red")
                                pickedUpItem = "RED KEY";
                            else
                                pickedUpItem = parts[1].ToUpper();
                            UpdateStory(pickedUpItem + " does not exist");
                        }
                    }
                    
                    
                }
                else
                {
                    UpdateStory("Invalid command. Please try again.");
                }
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
                else if (parts[0] == "commands")
                {
                    UpdateStory("Available Commands: ");
                    foreach (string command in commands)
                    {
                        UpdateStory(command);
                    }
                }
                else if (parts[0] == "inventory")
                {
                    if (GameManager.instance.inventory.Count > 0)
                    {
                        UpdateStory("Inventory: ");
                        foreach (string items in GameManager.instance.inventory)
                        {
                            UpdateStory(items);
                        }
                    }
                    else
                    {
                        UpdateStory("No items in inventory");
                    }
                }
                else if (parts[0] == "investigate")
                {

                    if (NavagationManager.instance.currentRoom.roomName == "Poster")
                    {
                        if (GameManager.instance.pickedUpItems.Contains("RED"))
                            UpdateStory("There is nothing under the poster");
                        else
                            UpdateStory("Beneath the clown poster there is a RED KEY");
                    }
                    else if (NavagationManager.instance.currentRoom.roomName == "Shield")
                    {
                        if (GameManager.instance.pickedUpItems.Contains("SHIELD"))
                            UpdateStory("The old chest is empty...");
                        else
                            UpdateStory("Inside of the old chest there is a SHIELD");
                    }
                    else if (NavagationManager.instance.currentRoom.roomName == "Dragon")
                    {
                        if (GameManager.instance.pickedUpItems.Contains("GOLDEN"))
                            UpdateStory("Too much gold to carry, youll have to come back with a wagon to carry it all");
                        else
                            UpdateStory("There is a GOLDEN KEY amongst the dragon's loot");
                    }
                    else
                        UpdateStory("There is nothing here...");
                    NavagationManager.instance.Unpack();
                    
                }

                else
                {
                    UpdateStory("Invalid command. Please try again.");
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

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleController : MonoBehaviour
{
    public Image background;
    public TMP_Text storyText; // the 
    public TMP_Text inputText; // part of the input field where user enters response
    public TMP_Text placeHolderText; // part of the input field for initial placeholder text
    public Text toggleText;

    private bool darkmode;
    private Toggle toggle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //if (PlayerPrefs.HasKey("darkmode"))
        darkmode = PlayerPrefs.GetInt("darkmode", 1) == 1 ? true : false ; //1 is the default = meaing true for darkmode
        
        toggle = GetComponent<Toggle>();
        SetTheme();

        toggle.onValueChanged.AddListener(UpdateTheme); //Respond to updates during the game
    }

    void UpdateTheme(bool isChecked)
    {
        darkmode = isChecked;
        PlayerPrefs.SetInt("darkmode", darkmode ? 1 : 0); //Terniary Operator to update our preference so it is stored correctly
        SetTheme();
    }
    
    void SetTheme()
    {
        if(darkmode)
        {
            toggle.isOn = true;
            background.color = Color.black;
            storyText.color = Color.white;  
            inputText.color = Color.white;
            placeHolderText.color = Color.white;
            toggleText.color = Color.white;
        }
        else
        {
            toggle.isOn = false;
            background.color = Color.white;
            storyText.color = Color.black;
            inputText.color = Color.black;
            placeHolderText.color = Color.black;
            toggleText.color = Color.black;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

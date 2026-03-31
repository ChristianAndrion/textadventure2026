using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeFontSize : MonoBehaviour
{
    public TMP_Text storyText;

    private float newFontSize;
    private float fontSize;
    private Slider fontSizeSlider;
    void Start()
    {
        
        fontSize = storyText.fontSize;

        fontSizeSlider = GetComponent<Slider>();
        newFontSize = PlayerPrefs.GetFloat("fontSize", 14) == 14 ? fontSizeSlider.value = 14 : fontSizeSlider.value = PlayerPrefs.GetFloat("fontSize");
        SetFontSize();

        fontSizeSlider.onValueChanged.AddListener(UpdateFontSize); //Respond to updates during the game
    }

    void UpdateFontSize(float value)
    {
        newFontSize = value;
        PlayerPrefs.SetFloat("fontSize",newFontSize);
        SetFontSize();
    }

    void SetFontSize()
    {
        storyText.fontSize = newFontSize;
    }

    
}

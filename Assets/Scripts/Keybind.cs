using System;
using UnityEngine;
using UnityEngine.UI;

public class Keybind : MonoBehaviour
{
    [Header("Objects")]
    public Text buttonTextOne;
    public Text buttonTextTwo;
    public Text buttonTextThree;
    public Text buttonTextFour;

    public Text audioText;
    public Slider slider;
    private AudioSource audio;

    private void Start()
    {
        audio = GetComponent<AudioSource>();

        PlayerPrefs.GetFloat("Volume", 1f);
    }

    private void Update()
    {
        Text[] buttonTexts = { buttonTextOne, buttonTextTwo, buttonTextThree, buttonTextFour };
        string[] keybindNames = { "KeybindOne", "KeybindTwo", "KeybindThree", "KeybindFour" };

        for (int i = 0; i < buttonTexts.Length; i++)
        {
            if (buttonTexts[i].text == "Awaiting Input")
            {
                foreach (KeyCode keycode in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(keycode))
                    {
                        string keyString = keycode.ToString();

                        // Convert special keys to readable symbols
                        /* switch (keycode)
                        {
                            case KeyCode.Comma:
                                keyString = ",";
                                break;
                            case KeyCode.Period:
                                keyString = ".";
                                break;
                            case KeyCode.Slash:
                                keyString = "/";
                                break;
                            case KeyCode.Backslash:
                                keyString = "\\";
                                break;
                            case KeyCode.Semicolon:
                                keyString = ";";
                                break;
                            case KeyCode.Quote:
                                keyString = "'";
                                break;
                            case KeyCode.LeftBracket:
                                keyString = "[";
                                break;
                            case KeyCode.RightBracket:
                                keyString = "]";
                                break;
                            case KeyCode.Minus:
                                keyString = "-";
                                break;
                            case KeyCode.Equals:
                                keyString = "=";
                                break;
                            case KeyCode.BackQuote:
                                keyString = "`";
                                break;
                        }*/

                        // Strip "Alpha" prefix for number keys
                        if (keyString.StartsWith("Alpha"))
                        {
                            keyString = keyString.Substring(5);
                        }

                        buttonTexts[i].text = keyString;
                        PlayerPrefs.SetString(keybindNames[i], keyString);
                        PlayerPrefs.Save();
                        break; // prevent multiple keys from being registered in one frame
                    }
                }
            }
        }
        float volume = audio.volume = slider.value;
        audioText.text = (slider.value * 100).ToString("F2") + "%";

        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();
    }

    public void ButtonOne()
    {
        buttonTextOne.text = "Awaiting Input";
    }

    public void ButtonTwo()
    {
        buttonTextTwo.text = "Awaiting Input";
    }

    public void ButtonThree()
    {
        buttonTextThree.text = "Awaiting Input";
    }

    public void ButtonFour()
    {
        buttonTextFour.text = "Awaiting Input";
    }
}

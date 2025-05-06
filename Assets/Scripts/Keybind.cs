using UnityEngine;
using UnityEngine.UI;
using System;

public class Keybind : MonoBehaviour
{
    [Header("Objects")]
    public Text buttonTextOne;
    public Text buttonTextTwo;
    public Text buttonTextThree;
    public Text buttonTextFour;

    private void Start()
    {

    }

    private void Update()
    {
        if(buttonTextOne.text == "Awaiting Input")
        {
            foreach(KeyCode keycode in Enum.GetValues(typeof(KeyCode)))
            {
                if(Input.GetKeyDown(keycode))
                {
                    buttonTextOne.text = keycode.ToString();
                    PlayerPrefs.SetString("KeybindOne", keycode.ToString());
                    PlayerPrefs.Save();
                }
            }
        }
        if(buttonTextTwo.text == "Awaiting Input")
        {
            foreach(KeyCode keycode in Enum.GetValues(typeof(KeyCode)))
            {
                if(Input.GetKeyDown(keycode))
                {
                    buttonTextTwo.text = keycode.ToString();
                    PlayerPrefs.SetString("KeybindTwo", keycode.ToString());
                    PlayerPrefs.Save();
                }
            }
        }
        if(buttonTextThree.text == "Awaiting Input")
        {
            foreach(KeyCode keycode in Enum.GetValues(typeof(KeyCode)))
            {
                if(Input.GetKeyDown(keycode))
                {
                    buttonTextThree.text = keycode.ToString();
                    PlayerPrefs.SetString("KeybindThree", keycode.ToString());
                    PlayerPrefs.Save();
                }
            }
        }
        if(buttonTextFour.text == "Awaiting Input")
        {
            foreach(KeyCode keycode in Enum.GetValues(typeof(KeyCode)))
            {
                if(Input.GetKeyDown(keycode))
                {
                    buttonTextFour.text = keycode.ToString();
                    PlayerPrefs.SetString("KeybindFour", keycode.ToString());
                    PlayerPrefs.Save();
                }
            }
        }
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
using UnityEngine;
using UnityEngine.UIElements;

public class MyUIManager : MonoBehaviour
{
    private Button myButton;

    void Start()
    {
        // Get root VisualElement from the UIDocument
        var root = GetComponent<UIDocument>().rootVisualElement;

        // Get the button by name
        myButton = root.Q<Button>("a");

        // Register a callback for when the button is clicked
        myButton.clicked += OnMyButtonClicked;
    }

    private void OnMyButtonClicked()
    {
        Debug.Log("Button was clicked!");
        // Do whatever you need here
    }
}


using UnityEngine;

public class DrawCard : MonoBehaviour
{
    public CardManager cardManager;

    public string cardName;

    public GameObject playedCards;
    AiOneCards aiCards;
    AiThreeCards aiThreeCards;
    RotateArrow rotateArrow;

    void Start()
    {
        // Find the CardManager in the scene
        cardManager = FindObjectOfType<CardManager>();
        aiCards = FindObjectOfType<AiOneCards>();
        aiThreeCards = FindObjectOfType<AiThreeCards>();
        rotateArrow = FindObjectOfType<RotateArrow>();

        playedCards = GameObject.Find("PlayedCards");


        if (cardManager == null)
        {
            Debug.LogError("CardManager not found in the scene!");
        }
        else
        {
            // Choose a random card and place it at (0, 0, 0)
            PlaceRandomCardAtStart();
        }
    }

    void Update()
    {
        if(cardName.StartsWith("red"))
        {
            Camera.main.backgroundColor = new Color(1f, 0.6f, 0.6f);
        }
        if(cardName.StartsWith("blue"))
        {
            Camera.main.backgroundColor = new Color(0.6f, 0.8f, 1f);
        }
        if(cardName.StartsWith("yellow"))
        {
            Camera.main.backgroundColor = new Color(1f, 1f, 0.6f);
        }
        if(cardName.StartsWith("green"))
        {
            Camera.main.backgroundColor = new Color(0.6f, 1f, 0.6f);
        }
    }

    void OnMouseUpAsButton()
    {
        if(cardManager.yourTurn)
        {
            cardManager.AddCard();
            cardManager.yourTurn = false;
        }
        if(cardManager.reversed)
        {
            aiThreeCards.aiThree = true;
            aiThreeCards.hasPlayedCard = false;
        }
        else
        { 
            aiCards.hasPlayedCard = false;
            aiCards.aiOne = true;
        }
        rotateArrow.Rotate();
    }

    void PlaceRandomCardAtStart()
    {
        if (cardManager.cardSprites == null || cardManager.cardSprites.Length == 0)
        {
            Debug.LogError("No card sprites assigned in CardManager!");
            return;
        }
        int randomIndex = Random.Range(0, cardManager.cardSprites.Length);

        // Create a card sprite with the name as the sprite
        GameObject newCard = new GameObject(cardManager.cardSprites[randomIndex].name);

        // Add the SpriteRenderer component to the new card
        SpriteRenderer spriteRenderer = newCard.AddComponent<SpriteRenderer>();

        // Add the Card script to handle card logic
        Card card = newCard.AddComponent<Card>();

        // Set the sprite to a random sprite from the list in CardManager
        spriteRenderer.sprite = cardManager.cardSprites[randomIndex];

        // Set the new card's position to (0, 0, 0)
        newCard.transform.position = Vector3.zero;

        newCard.transform.SetParent(playedCards.transform);

        cardName = newCard.name;
    }
}

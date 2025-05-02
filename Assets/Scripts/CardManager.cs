using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class CardManager : MonoBehaviour
{
    public Sprite[] cardSprites;            // List of possible card sprites
    public float cardSpacing = 0.5f;        // Space between cards
    public int totalCards;         // Initial number of cards
    public GameObject canvasObj;
    public GameObject drawPile;
    public GameObject playedCards;
    public TextMeshProUGUI textComponent;
    public bool unoClicked = false;

    public int playedCardsNum;
    public bool yourTurn = true;
    private AudioSource audio;

    AiOneCards aiCards;


    private float totalWidth;

    public bool reversed = false;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        Debug.Log(cardSprites.Length);

        aiCards = FindObjectOfType<AiOneCards>();

        for (int i = 0; i < 1; i++)
        {
            AddCard();
        }

        UpdateCardPositions();

        for(int i = 0; i < cardSprites.Length; i++)
        {
            if(i >= 0 && i <= 8)
            {
                cardSprites[i].name = "red_" + i.ToString();
            }
            else if (i == 9)
            {
                cardSprites[i].name = "Wild";
            }
            else if (i >= 10 && i <= 18)
            {
                cardSprites[i].name = "yellow_" + (i-10).ToString();
            }
            else if (i == 19)
            {
                cardSprites[i].name = "Wild";
            }
            else if (i >= 20 && i <= 28)
            {
                cardSprites[i].name = "blue_" + (i - 20).ToString();
            }
            else if (i == 29)
            {
                cardSprites[i].name = "Draw4";
            }
            else if(i >= 30 && i <= 38)
            {
                cardSprites[i].name = "green_" + (i - 30).ToString();
            }
            else if (i == 39)
            {
                cardSprites[i].name = "Draw4";
            }
            if(i >= 52 && i <= 60)
            {
                cardSprites[i].name = "red_" + (i - 43).ToString();
            }
            if(i >= 61 && i <= 69)
            {
                cardSprites[i].name = "yellow_" + (i - 52).ToString();
            }
            if(i >= 70 && i <= 78)
            {
                cardSprites[i].name = "blue_" + (i - 61).ToString();
            }
            if(i >= 79 && i <= 87)
            {
                cardSprites[i].name = "green_" + (i - 70).ToString();
            }

            switch (i)
            {
                // Skips
                case 40:
                    cardSprites[i].name = "red_skip";
                    break;
                case 41:
                    cardSprites[i].name = "yellow_skip";
                    break;
                case 42:
                    cardSprites[i].name = "blue_skip";
                    break;
                case 43:
                    cardSprites[i].name = "green_skip";
                    break;
                // draw two's
                case 44:
                    cardSprites[i].name = "red_draw2";
                    break;
                case 45:
                    cardSprites[i].name = "yellow_draw2";
                    break;
                case 46:
                    cardSprites[i].name = "blue_draw2";
                    break;
                case 47:
                    cardSprites[i].name = "green_draw2";
                    break;
                // Reverse's
                case 48:
                    cardSprites[i].name = "red_reverse";
                    break;
                case 49:
                    cardSprites[i].name = "yellow_reverse";
                    break;
                case 50:
                    cardSprites[i].name = "blue_reverse";
                    break;
                case 51:
                    cardSprites[i].name = "green_reverse";
                    break;
            }
        }
    }

    public void AddCard()
    {
        int currentCardCount = transform.childCount;

        // Dynamically reduce spacing if too many cards
        if (currentCardCount > 12)
        {
            cardSpacing -= 0.1f;
        }
        if (cardSpacing < 0.3f)
        {
            cardSpacing = 0.3f;
        }

        CreateCardSprite();
        totalCards++;
        UpdateCardPositions();
    }

    void CreateCardSprite()
    {
        if (cardSprites == null || cardSprites.Length == 0)
        {
            Debug.LogWarning("No card sprites assigned!");
            return;
        }

        int currentCardCount = transform.childCount;

        // Pick a random sprite
        int randomIndex = Random.Range(0, cardSprites.Length);
        Sprite selectedSprite = cardSprites[randomIndex];

        // Create a new GameObject for the card and name it based on the sprite name
        GameObject newCard = new GameObject(selectedSprite.name);

        Debug.Log(selectedSprite.name);

        // Get the SpriteRenderer component
        SpriteRenderer spriteRenderer = newCard.AddComponent<SpriteRenderer>();

        // Set the sprite to the randomly selected sprite
        spriteRenderer.sprite = selectedSprite;

        // Initialize the card
        Card cardScript = newCard.AddComponent<Card>();
        cardScript.Initialize(spriteRenderer);

        // Set sorting order (used to determine rendering order of cards)
        spriteRenderer.sortingOrder = totalCards;

        // Add a BoxCollider2D for interaction
        newCard.AddComponent<BoxCollider2D>();

        // Set the parent of the new card to the CardManager's transform
        newCard.transform.SetParent(transform);

        // Set the card's rotation to be the same as the parent (usually none)
        newCard.transform.localRotation = Quaternion.identity;
    }

    void Update()
    {
        if(totalCards == 0)
        {
            textComponent.text = "You Have Won The Game";
            StartCoroutine(EndGame());
            drawPile.SetActive(false);
            canvasObj.SetActive(true);
            playedCards.SetActive(false);
        }
        if(totalCards > 1)
        {
            unoClicked = false;
        }
    }

    public void UpdateCardPositions()
    {
        int cardCount = transform.childCount;
        totalWidth = (cardCount - 1) * cardSpacing;

        for (int i = 0; i < cardCount; i++)
        {
            GameObject card = transform.GetChild(i).gameObject;

            float xPos = (i * cardSpacing) - (totalWidth / 2f);
            card.transform.localPosition = new Vector3(xPos, 0f, 0); // Set Y to match your hover logic
        }
    }

    public void UnoClicked()
    {
        unoClicked = true;
    }
    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Start");
    }
}

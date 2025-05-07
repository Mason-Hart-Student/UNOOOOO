using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AiThreeCards : MonoBehaviour
{
    public GameObject drawPile;
    public Sprite[] cardSprites; // List of possible card sprites
    public Sprite[] cardBack;
    public float cardSpacing = 0.5f; // Space between the cards
    public int totalCards;
    public Text textComponent;
    public GameObject canvasObj;
    public Text aiName;
    public RotateArrow rotateArrow;

    public bool aiThree = false; // AI turn control (Set to true when it's the AI's turn)
    public bool hasPlayedCard = true;
    private string[] aiNames = new string[]
    {
        "Andrew",
        "Anthony",
        "Brian",
        "Charles",
        "Christopher",
        "Daniel",
        "David",
        "Donald",
        "Edward",
        "George",
        "James",
        "Jason",
        "Jeffery",
        "John",
        "Joseph",
        "Joshua",
        "Kenneth",
        "Kevin",
        "Mark",
        "Matthew",
        "Michael",
        "Paul",
        "Richard",
        "Robert",
        "Ronald",
        "Ryan",
        "Steven",
        "Thomas",
        "Timothy",
        "William",
        // Additional 75 names
        "Aaron",
        "Adam",
        "Alan",
        "Albert",
        "Alex",
        "Alexander",
        "Allen",
        "Arthur",
        "Austin",
        "Barry",
        "Benjamin",
        "Billy",
        "Blake",
        "Bradley",
        "Brandon",
        "Brent",
        "Bruce",
        "Bryan",
        "Caleb",
        "Carl",
        "Carter",
        "Chad",
        "Clarence",
        "Clifford",
        "Clyde",
        "Connor",
        "Craig",
        "Curtis",
        "Dale",
        "Darren",
        "Dean",
        "Dennis",
        "Derek",
        "Dominic",
        "Douglas",
        "Dustin",
        "Earl",
        "Eddie",
        "Eli",
        "Ethan",
        "Eugene",
        "Francis",
        "Frank",
        "Fred",
        "Gabriel",
        "Garry",
        "Gavin",
        "Gerald",
        "Glenn",
        "Gordon",
        "Grant",
        "Greg",
        "Harold",
        "Harry",
        "Henry",
        "Howard",
        "Ian",
        "Isaac",
        "Jack",
        "Jacob",
        "Jake",
        "Jared",
        "Jeremiah",
        "Jeremy",
        "Jerry",
        "Jesse",
        "Joel",
        "Jon",
        "Jonathan",
        "Jordan",
        "Justin",
        "Keith",
        "Kyle",
        "Lawrence",
        "Lee",
    };

    private CardManager cardManager;
    private DrawCard drawCard;

    private string[] cardPrefixes = new string[] { "red", "yellow", "blue", "green" };
    private float totalWidth;
    private Vector3 playCardPos = Vector3.zero;

    public GameObject playedCards;
    private string topCard;

    AiOneCards aiOneCards;
    AiTwoCards aiTwoCards;

    void Start()
    {
        int randomIndex = Random.Range(0, aiNames.Length);
        aiName.text = "Ai " + aiNames[randomIndex];
        cardManager = FindObjectOfType<CardManager>();
        drawCard = FindObjectOfType<DrawCard>();
        aiOneCards = FindObjectOfType<AiOneCards>();
        aiTwoCards = FindObjectOfType<AiTwoCards>();
        rotateArrow = FindObjectOfType<RotateArrow>();
        UpdateCardPositions();

        // Create some cards for AI at the start
        for (int i = 0; i < 7; i++)
        {
            AddCard();
        }
    }

    void LateUpdate()
    {
        UpdateTextColor();
        if (aiThree && !hasPlayedCard)
        {
            TryPlayCard();
        }
    }

    public void Erm()
    {
        hasPlayedCard = false;
        aiThree = true;
    }

    public void AddCard()
    {
        aiThree = false;
        if (totalCards > 12)
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

        GameObject newCard = new GameObject();
        SpriteRenderer spriteRenderer = newCard.AddComponent<SpriteRenderer>();
        Card card = newCard.AddComponent<Card>();
        card.Initialize(spriteRenderer);

        string randomPrefix = cardPrefixes[Random.Range(0, cardPrefixes.Length)];
        int randomNumber = Random.Range(0, 18);
        string cardName = randomPrefix + "_" + randomNumber;

        newCard.name = cardName;

        int randomIndex = Random.Range(0, cardSprites.Length);
        spriteRenderer.sprite = cardBack[0];

        spriteRenderer.sortingOrder = totalCards;
        newCard.transform.SetParent(transform);
        newCard.transform.rotation = transform.rotation;
        newCard.transform.localScale = new Vector3(0.3f, 0.3f, 1f);

        Debug.Log("Card Created: " + cardName);
    }

    void UpdateCardPositions()
    {
        // Check if there are any cards to position
        if (transform.childCount == 0)
            return;

        totalWidth = (totalCards - 1) * cardSpacing;

        for (int i = 0; i < totalCards; i++)
        {
            if (i >= transform.childCount)
                break; // Avoid accessing out-of-bounds

            GameObject card = transform.GetChild(i).gameObject;
            float xPos = (i * cardSpacing) - (totalWidth / 2);
            card.transform.localPosition = new Vector3(xPos, 0, 0);
        }
    }

    public void TryPlayCard()
    {
        StartCoroutine(PlayCardIfPossible());
    }

    private IEnumerator PlayCardIfPossible()
    {
        yield return new WaitForSeconds(1f);
        if (!aiThree || hasPlayedCard)
            yield break;
        if (cardManager == null)
            yield break;
        if (transform.childCount == 0)
            yield break;

        bool cardPlayed = false;
        string topCardName = drawCard.cardName;

        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject card = transform.GetChild(i).gameObject;
            string cardName = card.name;
            SpriteRenderer spriteRenderer = card.GetComponent<SpriteRenderer>();

            // Wild or Draw4
            if (cardName == "Wild" || cardName == "Draw4")
            {
                PlayCard(card, spriteRenderer, true);
                cardPlayed = true;
                break;
            }

            string[] cardParts = cardName.Split('_');
            string[] topCardParts = topCardName.Split('_');

            if (cardParts.Length >= 2 && topCardParts.Length >= 2)
            {
                if (cardParts[0] == topCardParts[0] || cardParts[1] == topCardParts[1])
                {
                    PlayCard(card, spriteRenderer);
                    cardPlayed = true;
                    break;
                }
            }
        }

        if (cardPlayed)
        {
            if (totalCards == 0)
            {
                drawPile.SetActive(false);
                textComponent.text = aiName.text + " Has Won The Game!";
                StartCoroutine(EndGame());
                canvasObj.SetActive(true);
                playedCards.SetActive(false);
            }
            UpdateCardPositions();
            if (cardManager.reversed)
            {
                aiTwoCards.aiTwo = true;
                aiTwoCards.hasPlayedCard = false;
                aiThree = false;
                hasPlayedCard = false;
                yield return null;
            }
            cardManager.yourTurn = true;
            aiThree = false;
            hasPlayedCard = true;
        }
        else
        {
            AddCard(); // Draw one card if nothing could be played
            aiThree = false;
            cardManager.yourTurn = true;
            hasPlayedCard = true;
        }
        rotateArrow.Rotate();
    }

    private void PlayCard(GameObject card, SpriteRenderer spriteRenderer, bool isSpecial = false)
    {
        cardManager.playedCardsNum++;
        card.transform.SetParent(playedCards.transform);
        card.transform.position = playCardPos;
        card.transform.rotation = isSpecial
            ? Quaternion.Euler(0, 0, -90)
            : Quaternion.Euler(0, 0, 0);
        drawCard.cardName = card.name;
        spriteRenderer.sortingOrder = cardManager.playedCardsNum;

        Sprite newSprite = GetCardSpriteByName(card.name);

        if (newSprite != null)
        {
            spriteRenderer.sprite = newSprite; // Set the sprite to the matched one
        }
        card.transform.localScale = Vector3.one;

        totalCards--;

        BoxCollider2D collider = card.GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            Destroy(collider);
        }

        Debug.Log("AI played card: " + card.name);
    }

    private Sprite GetCardSpriteByName(string cardName)
    {
        // Extract the color and number from the card name
        string[] cardParts = cardName.Split('_');
        if (cardParts.Length < 2)
            return null;

        string color = cardParts[0]; // The color part (red, blue, green, yellow)
        string number = cardParts[1]; // The number part (0-9)

        // Loop through the cardSprites array to find the matching sprite
        foreach (Sprite sprite in cardSprites)
        {
            if (sprite.name == cardName)
            {
                return sprite; // Return the matching sprite
            }
        }

        return null; // Return null if no matching sprite was found
    }

    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Start");
    }

    void UpdateTextColor()
    {
        Color darkGreen = new Color(0, 0.7f, 0);
        aiName.color = aiThree ? darkGreen : Color.black;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    public Sprite[] cardSprites;
    public float cardSpacing = 0.5f;
    public int totalCards;
    public GameObject canvasObj;
    public GameObject drawPile;
    public GameObject playedCards;
    public Text textComponent;
    public bool unoClicked = false;
    public int cardSelected = -1;
    public int playedCardsNum;
    public bool yourTurn = true;
    private AudioSource audio;

    private AiOneCards aiCards;
    private DrawCard drawCard;
    private Keybind keybind;

    private float totalWidth;

    public bool reversed = false;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        drawCard = FindObjectOfType<DrawCard>();
        keybind = FindObjectOfType<Keybind>();
        aiCards = FindObjectOfType<AiOneCards>();

        InitializeCardSprites();

        for (int i = 0; i < 7; i++)
        {
            AddCard();
        }
    }

    void InitializeCardSprites()
    {
        for (int i = 0; i < cardSprites.Length; i++)
        {
            switch (i)
            {
                case 9:
                    cardSprites[i].name = "Wild";
                    break;
                case 19:
                    cardSprites[i].name = "Wild";
                    break;
                case 29:
                    cardSprites[i].name = "Draw4";
                    break;
                case 39:
                    cardSprites[i].name = "Draw4";
                    break;
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
            }
            if (i >= 0 && i <= 8)
            {
                cardSprites[i].name = "red_" + i.ToString();
            }
            else if (i >= 10 && i <= 18)
            {
                cardSprites[i].name = "yellow_" + (i - 10).ToString();
            }
            else if (i >= 20 && i <= 28)
            {
                cardSprites[i].name = "blue_" + (i - 20).ToString();
            }
            else if (i >= 30 && i <= 38)
            {
                cardSprites[i].name = "green_" + (i - 30).ToString();
            }
            if (i >= 48 && i <= 56)
            {
                cardSprites[i].name = "red_" + (i - 39).ToString();
            }
            if (i >= 57 && i <= 65)
            {
                cardSprites[i].name = "yellow_" + (i - 48).ToString();
            }
            if (i >= 66 && i <= 74)
            {
                cardSprites[i].name = "blue_" + (i - 57).ToString();
            }
            if (i >= 75 && i <= 83)
            {
                cardSprites[i].name = "green_" + (i - 66).ToString();
            }
        }
    }

    public void AddCard()
    {
        int currentCardCount = transform.childCount;

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

        int randomIndex = UnityEngine.Random.Range(0, cardSprites.Length);
        Sprite selectedSprite = cardSprites[randomIndex];

        GameObject newCard = new GameObject(selectedSprite.name);

        SpriteRenderer spriteRenderer = newCard.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = selectedSprite;

        Card cardScript = newCard.AddComponent<Card>();
        cardScript.Initialize(spriteRenderer);

        BoxCollider2D boxCollider = newCard.AddComponent<BoxCollider2D>();

        newCard.transform.SetParent(transform);
        newCard.transform.localRotation = Quaternion.identity;
    }

    void Update()
    {
        if (totalCards == 0)
        {
            textComponent.text = "You Have Won The Game";
            StartCoroutine(EndGame());
            drawPile.SetActive(false);
            canvasObj.SetActive(true);
            playedCards.SetActive(false);
        }
        if (totalCards > 1)
        {
            unoClicked = false;
        }
        if (
            TryParseKey(keybind.buttonTextTwo.text, out KeyCode keyLeft)
                && Input.GetKeyDown(keyLeft)
            || Input.GetKeyDown(KeyCode.LeftArrow)
        )
        {
            cardSelected--;
        }

        if (
            TryParseKey(keybind.buttonTextOne.text, out KeyCode keyRight)
                && Input.GetKeyDown(keyRight)
            || Input.GetKeyDown(KeyCode.RightArrow)
        )
        {
            cardSelected++;
        }

        if (
            TryParseKey(keybind.buttonTextFour.text, out KeyCode keyDraw)
            && Input.GetKeyDown(keyDraw)
        )
        {
            drawCard.OnMouseUpAsButton();
        }

        if (
            TryParseKey(keybind.buttonTextThree.text, out KeyCode keyPlay)
            && Input.GetKeyDown(keyPlay)
        )
        {
            if (cardSelected >= transform.childCount)
            {
                cardSelected = transform.childCount - 1;
            }
            else
            {
                GameObject card = transform.GetChild(cardSelected).gameObject;
                card.GetComponent<Card>().OnMouseUpAsButton();
            }
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject card = transform.GetChild(i).gameObject;
            Vector3 pos = card.transform.position;
            if (i == cardSelected)
            {
                pos.y = -2.5f;
            }
            else
            {
                pos.y = -3.5f;
            }
            card.transform.position = pos;
        }
    }

    private bool TryParseKey(string input, out KeyCode key)
    {
        key = KeyCode.None;

        if (string.IsNullOrWhiteSpace(input))
            return false;

        // Handle single-digit numbers by converting to AlphaN
        if (input.Length == 1 && char.IsDigit(input[0]))
        {
            input = "Alpha" + input;
        }

        return Enum.TryParse(input, true, out key);
    }

    public void UpdateCardPositions()
    {
        int cardCount = transform.childCount;
        totalWidth = (cardCount - 1) * cardSpacing;

        for (int i = 0; i < cardCount; i++)
        {
            GameObject card = transform.GetChild(i).gameObject;

            float xPos = (i * cardSpacing) - (totalWidth / 2f);
            card.GetComponent<SpriteRenderer>().sortingOrder = i;
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

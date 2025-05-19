using System.Collections;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    public RotateArrow rotateArrow;
    private SpriteRenderer spriteRenderer;
    public GameObject playedCards;
    public int speed = 1;
    public TextMeshProUGUI textComponent;

    AiOneCards aiCards;
    AiTwoCards aiTwoCards;
    AiThreeCards aiThreeCards;

    public Canvas wildCanvas;

    CardManager cardManager;

    DrawCard drawCard;
    Vector3 playCardPos = Vector3.zero;

    private void Start()
    {
        drawCard = FindObjectOfType<DrawCard>();
        cardManager = FindObjectOfType<CardManager>();
        aiCards = FindObjectOfType<AiOneCards>();
        aiTwoCards = FindObjectOfType<AiTwoCards>();
        aiThreeCards = FindObjectOfType<AiThreeCards>();
        rotateArrow = FindObjectOfType<RotateArrow>();

        playedCards = GameObject.Find("PlayedCards");
        wildCanvas = GetComponent<Canvas>();
    }

    void Update() { }

    // Called when the card is initialized
    public void Initialize(SpriteRenderer renderer)
    {
        spriteRenderer = renderer;
    }

    void OnMouseEnter()
    {
        Vector3 pos = transform.position;
        pos.y = -2.5f;
        transform.position = pos;
        cardManager.cardSelected = transform.GetSiblingIndex();
    }
    
    void OnMouseExit()
    {
        Vector3 pos = transform.position;
        if (pos != playCardPos)
        {
            pos.y = -3.5f;
        }
        transform.position = pos;
        cardManager.cardSelected = -1;
    }

    public IEnumerator MoveCard(Vector2 target, float duration)
    {
        Vector2 start = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            transform.position = Vector2.Lerp(start, target, t);
            yield return null;
        }

        transform.position = target; // ensure it ends exactly at the target
    }

    public void OnMouseUpAsButton()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (transform.name == "Wild" && cardManager.yourTurn)
        {
            if (cardManager.totalCards == 1 && !cardManager.unoClicked)
            {
                cardManager.AddCard();
                cardManager.AddCard();
                return;
            }
            cardManager.yourTurn = false;
            cardManager.playedCardsNum++;
            cardManager.totalCards--;
            transform.SetParent(playedCards.transform);
            StartCoroutine(MoveCard(playCardPos, .5f));
            cardManager.UpdateCardPositions();
            drawCard.cardName = transform.name;
            GetComponent<SpriteRenderer>().sortingOrder = cardManager.playedCardsNum;

            BoxCollider2D collider = GetComponent<BoxCollider2D>();
            if (collider != null)
            {
                Destroy(collider);
            }
        }

        if (transform.name == "Draw4" && cardManager.yourTurn)
        {
            if (cardManager.totalCards == 1 && !cardManager.unoClicked)
            {
                cardManager.AddCard();
                cardManager.AddCard();
                return;
            }
            cardManager.yourTurn = false;
            cardManager.playedCardsNum++;
            cardManager.totalCards--;
            aiCards.DrawFour();
            transform.SetParent(playedCards.transform);
            StartCoroutine(MoveCard(playCardPos, .5f));
            cardManager.UpdateCardPositions();
            drawCard.cardName = transform.name;
            GetComponent<SpriteRenderer>().sortingOrder = cardManager.playedCardsNum;

            BoxCollider2D collider = GetComponent<BoxCollider2D>();
            if (collider != null)
            {
                Destroy(collider);
            }
        }
        // Split transform name and card name into color and number
        string[] transformParts = transform.name.Split('_');
        string[] cardNameParts = drawCard.cardName.Split('_');

        // Safety check for proper naming format
        if (transformParts.Length >= 2 && cardNameParts.Length >= 2)
        {
            string transformColor = transformParts[0];
            string transformNumber = transformParts[1];

            string cardColor = cardNameParts[0];
            string cardNumber = cardNameParts[1];

            // Allow play if either color or number matches
            if (
                transformColor == cardColor && cardManager.yourTurn
                || transformNumber == cardNumber && cardManager.yourTurn
            )
            {
                if (cardManager.totalCards == 1 && !cardManager.unoClicked)
                {
                    cardManager.AddCard();
                    cardManager.AddCard();
                    if (cardManager.reversed)
                    {
                        aiThreeCards.aiThree = true;
                        aiThreeCards.hasPlayedCard = false;
                    }
                    else
                    {
                        aiCards.aiOne = true;
                        aiCards.hasPlayedCard = false;
                    }
                    return;
                }
                cardManager.yourTurn = false;

                switch (transformNumber)
                {
                    case "draw2":
                        if (cardManager.reversed)
                        {
                            aiThreeCards.AddCard();
                            aiThreeCards.AddCard();
                            aiTwoCards.hasPlayedCard = false;
                            aiTwoCards.aiTwo = true;
                        }
                        else
                        {
                            aiCards.AddCard();
                            aiCards.AddCard();
                            aiTwoCards.hasPlayedCard = false;
                            aiTwoCards.aiTwo = true;
                        }
                        break;

                    case "skip":
                        aiTwoCards.hasPlayedCard = false;
                        aiTwoCards.aiTwo = true;
                        break;

                    case "reverse":
                        if (cardManager.reversed)
                        {
                            cardManager.reversed = false;
                            aiCards.hasPlayedCard = false;
                            aiCards.aiOne = true;
                        }
                        else
                        {
                            cardManager.reversed = true;
                            aiThreeCards.hasPlayedCard = false;
                            aiThreeCards.aiThree = true;
                        }
                        break;
                    default:
                        if (!cardManager.reversed)
                        {
                            aiCards.aiOne = true;
                            aiCards.hasPlayedCard = false;
                        }
                        else
                        {
                            aiThreeCards.aiThree = true;
                            aiThreeCards.hasPlayedCard = false;
                        }
                        break;
                }

                cardManager.playedCardsNum++;
                transform.SetParent(playedCards.transform);
                StartCoroutine(MoveCard(playCardPos, .5f));
                cardManager.UpdateCardPositions();
                cardManager.totalCards--;
                drawCard.cardName = transform.name;
                GetComponent<SpriteRenderer>().sortingOrder = cardManager.playedCardsNum;
                rotateArrow.Rotate();

                BoxCollider2D collider = GetComponent<BoxCollider2D>();
                if (collider != null)
                {
                    Destroy(collider);
                }
            }
        }
    }
}

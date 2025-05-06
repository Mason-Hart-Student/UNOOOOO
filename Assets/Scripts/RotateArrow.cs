using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateArrow : MonoBehaviour
{
    private CardManager cardManager;
    private bool previousReversed;
    private bool isRotating = false;
    AiOneCards aiOneCards;
    AiTwoCards aiTwoCards;
    AiThreeCards aiThreeCards;

    void Start()
    {
        cardManager = FindObjectOfType<CardManager>();
        previousReversed = cardManager.reversed;
        aiOneCards = FindObjectOfType<AiOneCards>();
        aiTwoCards = FindObjectOfType<AiTwoCards>();
        aiThreeCards = FindObjectOfType<AiThreeCards>();
    }

    void Update()
    {
    }
    
    public void Rotate()
    {
        StartCoroutine(Rotate90Degrees());
    }

    private IEnumerator Rotate90Degrees()
    {
        isRotating = true;
        float startRotation = transform.localEulerAngles.z;
        float targetRotation = 90f;
        if(aiThreeCards.aiThree)
        {
            targetRotation = -90f;
        }
        if(aiTwoCards.aiTwo)
        {
            targetRotation = 180f;
        }
        if(aiOneCards.aiOne)
        {
            targetRotation = 90f;
        }
        if(cardManager.yourTurn)
        {
            targetRotation = 0f;
        }
        float timeElapsed = 0;
        while (timeElapsed < 0.5f)
        {
            float t = timeElapsed / 0.5f;
            t = t * t * (3f - 2f * t); // Smoothstep
            transform.localEulerAngles = new Vector3(0, 0, Mathf.Lerp(startRotation, targetRotation, t));
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        isRotating = false;
    }
}

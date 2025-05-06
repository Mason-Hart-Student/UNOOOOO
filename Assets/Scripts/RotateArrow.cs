using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateArrow : MonoBehaviour
{
    private CardManager cardManager;
    private bool previousReversed;
    private bool isRotating = false;

    void Start()
    {
        cardManager = FindObjectOfType<CardManager>();
        previousReversed = cardManager.reversed;
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
        float targetRotation = cardManager.reversed ? startRotation - 90 : startRotation + 90;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw4Wild : MonoBehaviour
{
    DrawCard drawCard;

    public Canvas canvas;
    public Canvas canva;

    private void Start()
    {
        drawCard = FindObjectOfType<DrawCard>();
    }

    void Update()
    {
        if(drawCard.cardName == "Wild")
        {
            canvas.gameObject.SetActive(true);
        }
        if(drawCard.cardName == "Draw4")
        {
            canva.gameObject.SetActive(true);
        }
    }

    public void ChooseColor(int num)
    {
        switch(num)
        {
            case 1:
                drawCard.cardName = "red_0";
                break;
            case 2:
                drawCard.cardName = "blue_0";
                break;
            case 3:
                drawCard.cardName = "yellow_0";
                break;
            case 4:
                drawCard.cardName = "green_0";
                break;
        }
    }
}

using UnityEngine;
using TMPro;

public class Preposition : MonoBehaviour
{

    private enum Directions
    {
        On,
        Under, 
        Behind,
        InFrontOf,
        Above,
        Below,
        NextTo,
        In
    }

    private string[] prepositionsText =
    {
        "Auf",
        "Unter",
        "Hinter",
        "Vor",
        "Über",
        "An",
        "Neben",
        "In"
    };

    private GameObject square;
    private GameObject circle;
    private TMP_Text prepositionTxt;

    private Directions currentDirection;

    private float thresholdOn = 0.4f;

    private void Start()
    {
        square = GameObject.Find("Square");
        circle = GameObject.Find("Circle");
        prepositionTxt = GameObject.Find("Preposition").GetComponent<TMP_Text>();
        currentDirection = Directions.On;
    }

    private void Update()
    {
        prepositionTxt.text = prepositionsText[(int)currentDirection];
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (CheckPosition())
                currentDirection = (Directions)((int)currentDirection++);
        }
    }

    private bool CheckPosition()
    {
        switch (currentDirection)
        {
            case Directions.On:
                float supLimit = (square.transform.localScale.y - circle.transform.localScale.y) / 2;
                float distanceY = square.transform.position.y - circle.transform.position.y;
                float distanceX = square.transform.position.x - circle.transform.position.x;
                if (distanceY <= supLimit && distanceY >= supLimit - thresholdOn && 
                    distanceX <= square.transform.localScale.x / 2  && distanceX >= -square.transform.localScale.x / 2)
                    return true;
                break;
        }
        return false;
    }

}


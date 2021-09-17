using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager: MonoBehaviour
{

    private enum Directions
    {
        On,
        Under,
        Above,
        Behind,
        InFrontOf,
        NextTo,
        In
    }

    private string[] prepositionsText =
    {
        "Auf",
        "Unter",
        "Über",
        "Hinter",
        "Vor",
        "Neben",
        "In"  
    };

    private GameObject square;
    private GameObject circle;
    private TMP_Text prepositionTxt;
    private TMP_Text mistakeText;


    private Directions currentDirection;
    private SpriteRenderer circleSpriteRenderer;

    private bool behind;
    private const int sortingBehind = -2;
    private const int sortingFront = 2;

    private const float thresholdOn = 0.4f;

    private IEnumerator disableMistakeCorutine;

    private void Start()
    {
        square = GameObject.Find("Square");
        circle = GameObject.Find("Circle");
        prepositionTxt = GameObject.Find("Preposition").GetComponent<TMP_Text>();
        mistakeText = GameObject.Find("Mistake").GetComponent<TMP_Text>();
        circleSpriteRenderer = circle.GetComponent<SpriteRenderer>();
        currentDirection = Directions.On;
        mistakeText.gameObject.SetActive(false);
    }

    private void Update()
    {
        prepositionTxt.text = prepositionsText[(int)currentDirection];
        ForDebuging();
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (behind)
            {
                circleSpriteRenderer.sortingOrder = sortingFront;
                circle.transform.localScale = Vector3.one;
                behind = false;
            }
            else
            {
                circleSpriteRenderer.sortingOrder = sortingBehind;
                circle.transform.localScale = 0.8f * Vector3.one;
                behind = true; 
            }        
        }
    }

    private void ForDebuging()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            currentDirection = (Directions)0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentDirection = (Directions)1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentDirection = (Directions)2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentDirection = (Directions)3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentDirection = (Directions)4;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            currentDirection = (Directions)5;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            currentDirection = (Directions)6;
        }
    }

    private bool CheckPosition()
    {
        float distanceX, distanceY;
        float supLimitX = (square.transform.localScale.x + circle.transform.localScale.x) / 2;
        float supLimitY = (square.transform.localScale.y + circle.transform.localScale.y) / 2;

        switch (currentDirection)
        {
            case Directions.On:
                distanceX = circle.transform.position.x - square.transform.position.x;
                distanceY = circle.transform.position.y - square.transform.position.y;
                
                if (distanceY <= supLimitY && distanceY >= supLimitY - thresholdOn &&  Mathf.Abs(distanceX) <= square.transform.localScale.x / 2)
                    return true;
                break;

            case Directions.Under:
                distanceX = square.transform.position.x - circle.transform.position.x;
                distanceY = square.transform.position.y - circle.transform.position.y;

                if (distanceY >= supLimitY && Mathf.Abs(distanceX) <= square.transform.localScale.x / 2)
                    return true;
                break;

            case Directions.Above:
                distanceX = circle.transform.position.x - square.transform.position.x;
                distanceY = circle.transform.position.y - square.transform.position.y;

                if (distanceY >= supLimitY && Mathf.Abs(distanceX) <= square.transform.localScale.x / 2)
                    return true;
                break;

            case Directions.NextTo:
                distanceX = circle.transform.position.x - square.transform.position.x;
                distanceY = circle.transform.position.y - square.transform.position.y;

                if (Mathf.Abs(distanceX) >= supLimitX && Mathf.Abs(distanceY) <= square.transform.localScale.y / 2)
                    return true;
                break;

            case Directions.Behind:
                distanceX = circle.transform.position.x - square.transform.position.x;
                distanceY = circle.transform.position.y - square.transform.position.y;
                if (circleSpriteRenderer.sortingOrder == sortingBehind && Mathf.Abs(distanceX) <= supLimitX && Mathf.Abs(distanceY) <= supLimitY)
                    return true;
                break;

            case Directions.InFrontOf:
                distanceX = circle.transform.position.x - square.transform.position.x;
                distanceY = circle.transform.position.y - square.transform.position.y;
                if (circleSpriteRenderer.sortingOrder == sortingFront && Mathf.Abs(distanceX) <= supLimitX && Mathf.Abs(distanceY) <= supLimitY)
                    return true;
                break;

            // TO DO: case In + case Default
        }
        return false;
    }

    public void Check()
    {
        if (disableMistakeCorutine != null)
            StopCoroutine(disableMistakeCorutine);

        if (CheckPosition())
        {
            ChangeDirection();
            //TO DO: adaugare efect corect/fals + tranzitie
        }
        else
        {
            mistakeText.gameObject.SetActive(true);
            mistakeText.text = "The object is in the wrong place!!! Try again";
            disableMistakeCorutine = DisableMistake();
            StartCoroutine(disableMistakeCorutine);
        }
    }

    private void ChangeDirection()
    {
        currentDirection++;
        // TO DO: random direction
    }

    private IEnumerator DisableMistake()
    {
        yield return new WaitForSeconds(2f);
        mistakeText.gameObject.SetActive(false);
    }
}

//TO DO: de adaugat efecte audio


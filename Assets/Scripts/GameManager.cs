using UnityEngine;
using TMPro;
using System.Collections;
using System;
using Random = UnityEngine.Random;
using UnityEngine.UI;

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

    private enum Languages
    {
        ENG,
        DEU
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
    private TMP_Text infoText;
    private TMP_Text volumeText;

    private Text checkButtonText;  //TO DO: de schimbat in Button TMP

    private Directions currentDirection;
    private SpriteRenderer circleSpriteRenderer;

    private Languages currentLanguage;

    private const int sortingBehind = -2;
    private const int sortingMiddle= 0;
    private const int sortingFront = 2;

    private readonly Vector3 behindLayer = new Vector3(0.6f, 0.6f, 0.6f);
    private readonly Vector3 middleLayer = new Vector3(0.8f, 0.8f, 0.8f);
    private readonly Vector3 frontLayer = new Vector3(1f, 1f, 1f);

    private const float thresholdOn = 0.4f;

    private IEnumerator disableMistakeCoroutine;

    private float volume = 0.5f;
    private AudioManager audioManager;
    private Slider slider;

    private void Start()
    {
        square = GameObject.FindGameObjectWithTag("box");
        circle = GameObject.FindGameObjectWithTag("player");
        circle.transform.localScale = frontLayer;
        prepositionTxt = GameObject.Find("Preposition").GetComponent<TMP_Text>();
        mistakeText = GameObject.Find("Mistake").GetComponent<TMP_Text>();
        checkButtonText = GameObject.Find("CheckButton").GetComponentInChildren<Text>();
        infoText = GameObject.Find("Info").GetComponent<TMP_Text>();
        volumeText = GameObject.Find("Volume").GetComponent<TMP_Text>();
        circleSpriteRenderer = circle.GetComponent<SpriteRenderer>();
        circleSpriteRenderer.sortingOrder = sortingFront;
        ChangeDirection();
        mistakeText.gameObject.SetActive(false);
        currentLanguage = Languages.ENG;
        audioManager = FindObjectOfType<AudioManager>();
        audioManager.SetVolume(volume);
        slider = FindObjectOfType<Slider>();
        slider.value = volume;
    }

    private void Update()
    {
        ForDebuging();

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            switch (circleSpriteRenderer.sortingOrder)
            {
                case sortingBehind:
                    circleSpriteRenderer.sortingOrder = sortingMiddle;
                    circle.transform.localScale = middleLayer;
                    break;

                case sortingMiddle:
                    circleSpriteRenderer.sortingOrder = sortingFront;
                    circle.transform.localScale = frontLayer;
                    break;

                case sortingFront:
                    //TO DO: adaugat un efect
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            switch (circleSpriteRenderer.sortingOrder)
            {
                case sortingBehind:
                    //TO DO: adaugat un efect
                    break;

                case sortingMiddle:
                    circleSpriteRenderer.sortingOrder = sortingBehind;
                    circle.transform.localScale = behindLayer;
                    break;

                case sortingFront:
                    circleSpriteRenderer.sortingOrder = sortingMiddle;
                    circle.transform.localScale = middleLayer;
                    break;
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
        if (currentLanguage == Languages.ENG)
            prepositionTxt.text = "Place the circle correctly for: " + prepositionsText[(int)currentDirection];
        if (currentLanguage == Languages.DEU)
            prepositionTxt.text = "Setzen Sie das Objekt fur: " + prepositionsText[(int)currentDirection];
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

            case Directions.In:
                //TO DO; de adaugat sprite-uri pentru box si modificat "0.1f"
                float squareX = square.transform.localScale.x / 2;
                float squareY = square.transform.localScale.y / 2;
                float circleX = circle.transform.localScale.x / 2;
                float circleY = circle.transform.localScale.y / 2;
                float squareLeftSide = square.transform.position.x - squareX;
                float squareRightSide = square.transform.position.x + squareX - 0.1f;
                float squareTopSide = square.transform.position.y + squareY;
                float squareBottomSide = square.transform.position.y - squareY - 0.1f;
                float circleLeftSide = circle.transform.position.x - circleX;
                float circleRightSide = circle.transform.position.x + circleX;
                float circleTopSide = circle.transform.position.y + circleY;
                float circleBottomSide = circle.transform.position.y - circleY;

                if (circleSpriteRenderer.sortingOrder == sortingMiddle)
                {
                    if (squareLeftSide <= circleLeftSide && squareRightSide >= circleRightSide)
                    {
                        if (squareTopSide >= circleTopSide && squareBottomSide <= circleBottomSide)
                        {
                            return true;
                        }    
                    }
                }
                break;

            default:
                throw new ArgumentException("Wrong direction");
            
        }
        return false;
    }

    public void Check()
    {
        if (disableMistakeCoroutine != null)
            StopCoroutine(disableMistakeCoroutine);

        if (CheckPosition())
        {
            ChangeDirection();
            audioManager.PlayCorrectAnswer();
            //TO DO: adaugare tranzitie
        }
        else
        {
            audioManager.PlayWrongAnswer();
            mistakeText.gameObject.SetActive(true);
            disableMistakeCoroutine = DisableMistake();
            StartCoroutine(disableMistakeCoroutine);
        }
    }

    private IEnumerator DisableMistake()
    {
        yield return new WaitForSeconds(2f);
        mistakeText.gameObject.SetActive(false);
    }

    private void ChangeDirection()
    {
        Directions tempDirection = (Directions)Random.Range(0, 6);
        while (currentDirection == tempDirection)
            tempDirection = (Directions)Random.Range(0, 6);
        currentDirection = tempDirection;
        if (currentLanguage == Languages.ENG)
            prepositionTxt.text = "Place the circle correctly for: " + prepositionsText[(int)currentDirection];
        if (currentLanguage == Languages.DEU)
            prepositionTxt.text = "Setzen Sie das Objekt fur: " + prepositionsText[(int)currentDirection];
    }

    

    public void ChangeLanguage()
    {
        TMP_Dropdown dropdown = FindObjectOfType<TMP_Dropdown>();
        switch (dropdown.value)
        {
            case (int)Languages.ENG:
                prepositionTxt.text = "Place the circle correctly for: " + prepositionsText[(int)currentDirection];
                mistakeText.text = "The object is in the wrong place!!! Try again";
                infoText.text = "bubululuuuu";
                currentLanguage = Languages.ENG;
                checkButtonText.text = "Check";
                volumeText.text = "Volume";
                break;

            case (int)Languages.DEU:
                prepositionTxt.text = "Setzen Sie das Objekt fur: " + prepositionsText[(int)currentDirection];
                mistakeText.text = "Das Objekt ist nicht richtig gesetzt!!! Versuchen Sie wieder";
                infoText.text = "blabalalallala";
                checkButtonText.text = "Verifizieren";
                currentLanguage = Languages.DEU;
                volumeText.text = "Lautstarke";
                break;

            default:
                throw new ArgumentException("Wrong language");
        }
    }

    public void ChangeVolume()
    {
        volume = slider.value;
        audioManager.SetVolume(volume);
    }
}

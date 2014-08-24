using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;

public class GameManager : OmegaObject {

    public GameObject secondSeedShield;
    public GameObject firstSeedShield;

    public float secondShieldSpacing;
    public float firstShieldSpacing;

    public float secondShieldSpeed;
    public float firstShieldSpeed;

    public int timeLimit; // in seconds

    public GameObject timeText;
    public GameObject healthText;

    public GameObject[] introElements;

    public GameObject endGameBodyText;
    public GameObject endGameLargeText;

    public AudioClip explosionClip;
    public AudioClip launchClip;

    private List<GameObject> firstShields = new List<GameObject>();
    private List<GameObject> secondShields = new List<GameObject>();
    private Vector3 secondSeedShieldPosition;
    private Vector3 firstSeedShieldPosition;

    private float elapsedTime = 0;

    private int planetHealth = 100; // important!

    private bool gameOver = false;

	// Use this for initialization
	void Start () 
    {
        LeanTween.init(2000);

        // set the time scale to 0 until the user dismisses the intro
        Time.timeScale = 0f;

        // spawn the shields

        this.secondSeedShieldPosition = secondSeedShield.transform.position;
        Destroy(secondSeedShield);

        float startingXPosition = secondSeedShieldPosition.x - 2 * secondShieldSpacing;

        for (int i = 0; i < 5; i++)
        {
            Vector2 currentPosition = new Vector2(
                startingXPosition + i * secondShieldSpacing,
                secondSeedShieldPosition.y);

            GameObject newShield = Instantiate(Resources.Load("Shield"), currentPosition, Quaternion.identity) as GameObject;
            secondShields.Add(newShield);
        }

        this.firstSeedShieldPosition = firstSeedShield.transform.position;
        Destroy(firstSeedShield);

        startingXPosition = firstSeedShieldPosition.x - 2 * firstShieldSpacing;

        for (int i = 0; i < 5; i++)
        {
            Vector2 currentPosition = new Vector2(
                startingXPosition + i * firstShieldSpacing,
                firstSeedShieldPosition.y);

            GameObject newShield = Instantiate(Resources.Load("Shield"), currentPosition, Quaternion.identity) as GameObject;
            firstShields.Add(newShield);
        }

        
	}
	
	// Update is called once per frame
	void Update () 
    {
        // update timer
        elapsedTime += Time.deltaTime;

        timeText.GetComponent<Text>().text = Mathf.Max(0, (timeLimit - elapsedTime)).ToString("0");

        if (elapsedTime > timeLimit)
        {
            EndGame();
        }


        // move the shields

        foreach (GameObject shield in secondShields)
        {
            shield.transform.position = new Vector3(
                shield.transform.position.x + secondShieldSpeed * Time.deltaTime,
                shield.transform.position.y,
                shield.transform.position.z);

            if (shield.transform.position.x > this.secondSeedShieldPosition.x + 2 * secondShieldSpacing)
            {
                shield.transform.position = new Vector3(
                    secondSeedShieldPosition.x - 2 * secondShieldSpacing,
                    shield.transform.position.y,
                    shield.transform.position.z);
            }
        }

        foreach (GameObject shield in firstShields)
        {
            shield.transform.position = new Vector3(
                shield.transform.position.x + firstShieldSpeed * Time.deltaTime,
                shield.transform.position.y,
                shield.transform.position.z);

            if (shield.transform.position.x > this.firstSeedShieldPosition.x + 2 * firstShieldSpacing)
            {
                shield.transform.position = new Vector3(
                    firstSeedShieldPosition.x - 2 * firstShieldSpacing,
                    shield.transform.position.y,
                    shield.transform.position.z);
            }
        }
	}

    void EndGame()
    {
        StartCoroutine(EndGameRoutine());
    }

    public void ChangePlanetHealth(int delta)
    {
        Debug.Log(string.Format("Changing planet health with delta {0}", delta));

        if (planetHealth + delta > 0 && planetHealth + delta <= 100)
        {
            planetHealth += delta;
        }
        else if (planetHealth + delta <= 0)
        {
            planetHealth = 0;
        }
        else if (planetHealth + delta > 100)
        {
            planetHealth = 100;
        }

        healthText.GetComponent<Text>().text = string.Format("{0}%", planetHealth);

        if (planetHealth == 0) EndGame();
    }

    private void StartGame()
    {
        StartCoroutine(StartGameRoutine());
    }

    IEnumerator StartGameRoutine()
    {
        if (!gameOver)
        {
            // start button clicked, start the game

            // fade intro stuff away

            foreach (GameObject element in introElements)
            {
                if (element.name == "StartButton")
                {
                    element.GetComponent<Image>().color = new Color(0, 0, 0, 0);
                }
                else if (element.GetComponent<Image>() != null)
                {
                    var image = element.GetComponent<Image>();
                    image.CrossFadeAlpha(0f, 0.5f, true);
                }
                else if (element.GetComponent<Text>() != null)
                {
                    var text = element.GetComponent<Text>();
                    text.CrossFadeAlpha(0f, 0.5f, true);
                }
            }

            //yield return new WaitForSeconds(0.5f);

            // set time scale back to normal
            Time.timeScale = 1f;

            // fucking Unity can't even wobble stars right
            foreach (GameObject star in GameObject.FindGameObjectsWithTag("star"))
            {
                star.GetComponent<Star>().StartWobble();
            }
        }
        else
        {
            Application.LoadLevel(Application.loadedLevel);
        }

        yield return 0;
    }

    IEnumerator EndGameRoutine()
    {
        gameOver = true;

        bool gameWon;
        if (planetHealth > 0) gameWon = false;
        else gameWon = true;

        // put new text and stuff on the intro

        string titleText;
        switch (gameWon)
        {
            case true:
                titleText = "Congratulations!";
                break;

            case false:
                titleText = "Game Over";
                break;

            default:
                titleText = "Game Over";
                break;
        }
        
        // fade intro stuff back in

        foreach (GameObject element in introElements)
        {
            if (element.name == "StartButton")
            {
                element.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            }
            else if (element.GetComponent<Image>() != null)
            {
                var image = element.GetComponent<Image>();
                image.CrossFadeAlpha(1f, 0.5f, true);
            }
            else if (element.GetComponent<Text>() != null)
            {
                if (element.name != "BodyText")
                {
                    var text = element.GetComponent<Text>();
                    text.CrossFadeAlpha(1f, 0.5f, true);

                    if (element.name == "Title") text.text = titleText;
                    else if (element.name == "ButtonText") text.text = "Play again";
                }
                else
                {
                    string bodyText;
                    if (gameWon) bodyText = "Yay, you won! Here's your score. Go brag about it.";
                    else bodyText = "Aw, too bad. Here's your score. Better luck next time!";

                    endGameBodyText.GetComponent<Text>().text = bodyText;
                    endGameLargeText.GetComponent<Text>().text = string.Format("{0} hits in {1} seconds", 10 - planetHealth / 10, (int)elapsedTime);

                    //endGameBodyText.GetComponent<Text>().CrossFadeAlpha(1f, 0.5f, true);
                    //endGameLargeText.GetComponent<Text>().CrossFadeAlpha(1f, 0.5f, true);     

                    endGameBodyText.GetComponent<Text>().color = new Color(0, 0, 0, 1);
                    endGameLargeText.GetComponent<Text>().color = new Color(0, 0, 0, 1);

                    //LeanTween.value(gameObject, SetGameOverTextAlpha, 0f, 1f, 0.5f);
                }
            }
        }

        Time.timeScale = 0f;

        yield return 0;
    }

    private void SetGameOverTextAlpha(float a)
    {
        Color newColor = new Color(
            endGameBodyText.GetComponent<Text>().color.r,
            endGameBodyText.GetComponent<Text>().color.g,
            endGameBodyText.GetComponent<Text>().color.b,
            a);

        endGameBodyText.GetComponent<Text>().color = newColor;
        endGameLargeText.GetComponent<Text>().color = newColor;
    }

    public void PlaySound(string sound)
    {
        switch (sound)
        {
            case "explosion":
                AudioSource.PlayClipAtPoint(explosionClip, Vector3.zero);
                break;

            case "launch":
                AudioSource.PlayClipAtPoint(launchClip, Vector3.zero);
                break;
        }
    }
}

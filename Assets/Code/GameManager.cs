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

    private List<GameObject> firstShields = new List<GameObject>();
    private List<GameObject> secondShields = new List<GameObject>();
    private Vector3 secondSeedShieldPosition;
    private Vector3 firstSeedShieldPosition;

    private float elapsedTime = 0;

    private int planetHealth = 100; // important!

	// Use this for initialization
	void Start () 
    {
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
}

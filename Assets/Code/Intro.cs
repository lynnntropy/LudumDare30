using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class Intro : MonoBehaviour {

    public GameObject gameName;
    public GameObject line1;
    public GameObject line2;

    public GameObject playButton;
    public GameObject playButtonText;

    public Color finalBgColor;

    public GameObject blackPanel;

	// Use this for initialization
	void Start () 
    {
        // because fuck the rules
        Screen.SetResolution(960, 500, false);

        StartCoroutine(IntroRoutine());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator IntroRoutine()
    {
        blackPanel.GetComponent<RectTransform>().localScale = Vector3.zero;

        gameName.GetComponent<Text>().CrossFadeAlpha(0f, 0f, true);
        line1.GetComponent<Text>().CrossFadeAlpha(0f, 0f, true);
        line2.GetComponent<Text>().CrossFadeAlpha(0f, 0f, true);
        
        playButton.GetComponent<Image>().CrossFadeAlpha(0f, 0f, true);
        playButtonText.GetComponent<Text>().CrossFadeAlpha(0f, 0f, true);
        

        yield return new WaitForSeconds(1f);

        gameName.GetComponent<Text>().CrossFadeAlpha(1f, 1.5f, true);

        yield return new WaitForSeconds(2f);

        line1.GetComponent<Text>().CrossFadeAlpha(1f, 1f, true);

        yield return new WaitForSeconds(0.5f);

        line2.GetComponent<Text>().CrossFadeAlpha(1f, 1f, true);

        yield return new WaitForSeconds(3f);

        line1.GetComponent<Text>().CrossFadeAlpha(0f, 1f, true);
        line2.GetComponent<Text>().CrossFadeAlpha(0f, 1f, true);

        yield return new WaitForSeconds(1.5f);

        // move the game name to the top of the screen

        LeanTween.moveY(gameName, Screen.height - 100, 2f)
            .setEase(LeanTweenType.easeInOutQuad);

        LeanTween.value(gameObject, ChangeBackgroundColor, 0f, 1f, 2f)
            .setEase(LeanTweenType.easeInOutQuad);

        yield return new WaitForSeconds(2.5f);

        playButton.GetComponent<Image>().CrossFadeAlpha(1f, 1f, true);
        playButtonText.GetComponent<Text>().CrossFadeAlpha(1f, 1f, true);

        yield return 0;
    }

    private void ChangeBackgroundColor(float f)
    {
        Camera.main.backgroundColor = Color.Lerp(Color.black, finalBgColor, f);
    }

    private void StartGame()
    {
        Debug.Log("Starting game!");

        blackPanel.GetComponent<RectTransform>().localScale = Vector3.one;
        //blackPanel.GetComponent<Image>().color = new Color(0, 0, 0, 1);
        //blackPanel.GetComponent<Image>().CrossFadeAlpha(1f, 1f, true);

        LeanTween.value(gameObject, SetBlackPanelAlpha, 0f, 1f, 1f)
            .setOnComplete(delegate() {

                Application.LoadLevel("main");

            });
    }

    private void SetBlackPanelAlpha(float a)
    {
        blackPanel.GetComponent<Image>().color = new Color(0, 0, 0, a);
    }
}

using UnityEngine;
using System.Collections;

public class Star : MonoBehaviour {

    public float rotateRange;

	// Use this for initialization
	void Start () {

        StartWobble();
	
	}
	
	// Update is called once per frame
	void Update () {

        if (!LeanTween.isTweening(gameObject))
        {
            Debug.Log("Star wasn't tweening, starting tween");
            StartWobble();
        }
	
	}

    public void StartWobble()
    {
        Debug.Log("Starting star wobble :)");

        LeanTween.cancel(gameObject);

        LeanTween.rotateZ(gameObject, transform.eulerAngles.z + rotateRange, 1f)
            .setEase(LeanTweenType.easeInOutQuad)
            .setLoopPingPong();       
    }
}

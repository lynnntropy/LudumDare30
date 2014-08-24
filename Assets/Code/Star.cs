using UnityEngine;
using System.Collections;

public class Star : MonoBehaviour {

    public float rotateRange;

	// Use this for initialization
	void Start () {

        LeanTween.rotateZ(gameObject, transform.eulerAngles.z + rotateRange, 1f)
            .setEase(LeanTweenType.easeInOutQuad)
            .setLoopPingPong();       
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

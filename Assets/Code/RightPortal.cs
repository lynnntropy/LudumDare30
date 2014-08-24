using UnityEngine;
using System.Collections;

public class RightPortal : BasePortal 
{
    public GameObject leftPortal;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D missile)
    {
        if (missile.gameObject.GetComponent<Missile>() != null)
        {
            // object is a missile
            Teleport(missile.gameObject);
        }
    }

    private void Teleport(GameObject missile)
    {
        float animationTime = 0.2f;

        LeanTween.alpha(missile, 0f, animationTime)
            .setEase(LeanTweenType.easeOutQuad)
            .setOnComplete(delegate()
        {

            float xDistance = missile.transform.position.x - transform.position.x;
            float missileAngle = missile.transform.rotation.eulerAngles.z;

            float newAngle = ReverseAngle(missileAngle);

            Vector3 newPosition = new Vector3(
                leftPortal.transform.position.x - xDistance,
                missile.transform.position.y,
                leftPortal.transform.position.z - 5);

            Vector3 newRotation = new Vector3(0, 0, newAngle);

            missile.transform.position = newPosition;
            missile.transform.eulerAngles = newRotation;

            LeanTween.alpha(missile, 1f, animationTime).setEase(LeanTweenType.easeOutQuad);

        });
        

    }

    private float ReverseAngle (float angle)
    {

        float newAngle = 180;
 
        if (angle < 180)
        {
            newAngle = 180 - angle;
        }
        else if (angle < 360)
        {
            newAngle = 360 - angle + 180;
        }

        return newAngle;
    }
}

using UnityEngine;
using System.Collections;

public class Missile : OmegaObject {

    public float movementSpeed;
    public float rotationSpeed;

    private GameObject leftCamera;
    private GameObject rightCamera;

    private Vector2 velocityVector;
    private float spawnTime;

    private GameManager gameManager;

    private GameObject missileExplosion;

	void Start () 
    {
        velocityVector = transform.up * movementSpeed;
        spawnTime = Time.timeSinceLevelLoad;

        leftCamera = GameObject.Find("Left Camera");
        rightCamera = GameObject.Find("Right Camera");

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        missileExplosion = GameObject.Find("MissileExplosion");
	}
	

	void Update () 
    {
        velocityVector = transform.up * movementSpeed;	

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rigidbody2D.angularVelocity += rotationSpeed;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            rigidbody2D.angularVelocity -= rotationSpeed;
        }

        if (Time.timeSinceLevelLoad > spawnTime + 1f && !this.customCollisionEnabled)
        {
            this.customCollisionEnabled = true;
        }	

        if (Vector2.Distance(leftCamera.transform.position, transform.position) > 7 && Vector2.Distance(rightCamera.transform.position, transform.position) > 7)
        {
            Destroy(gameObject);
        }
	}


    void FixedUpdate()
    {
        gameObject.rigidbody2D.velocity = velocityVector;
    }

    void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.gameObject.GetComponent<PlayerBase>() != null)
        {
            // entered the player base trigger
            if (customCollisionEnabled)
            {
                Explode();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Planet")
        {
            // collided with the planet!

            gameManager.ChangePlanetHealth(-10);
            Explode();
        }
        else if (collision.gameObject.name.Contains("Shield"))
        {
            // hit a shield :(

            Explode();
        }
    }

    void Explode()
    {
        missileExplosion.transform.position = transform.position;
        missileExplosion.GetComponent<ParticleSystem>().Play();

        Destroy(gameObject);
    }
}

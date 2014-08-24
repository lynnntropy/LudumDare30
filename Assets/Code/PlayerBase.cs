using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerBase : OmegaObject 
{
    public float spawnInterval;
    public GameObject spawnerParent;
    public Vector3 spawnOffset;

    public GameObject muzzleFlash;

    private float lastSpawnTime = 0f;
    private List<GameObject> spawners = new List<GameObject>();

	void Start () 
    {
	    foreach (Transform spawner in spawnerParent.transform)
        {
            spawners.Add(spawner.gameObject);
        }
	}	
	
	void Update () 
    {
	    if (Time.timeSinceLevelLoad > lastSpawnTime + spawnInterval)
        {
            lastSpawnTime = Time.timeSinceLevelLoad;

            // randomly select a spawner to launch from

            int randomIndex = Random.Range(0, spawners.Count);
            GameObject randomSpawner = spawners[randomIndex];

            // launch ze missile
            SpawnMissile(randomSpawner);
        }
	}

    private void SpawnMissile(GameObject spawner)
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().PlaySound("launch");

        Vector3 spawnPosition = new Vector3(
            spawner.transform.position.x,
            spawner.transform.position.y,
            spawner.transform.position.z - 5);

        spawnPosition = spawnPosition + spawnOffset;

        GameObject newMissile = Instantiate(Resources.Load("Missile"), spawnPosition, Quaternion.identity) as GameObject;

        // TODO spawn some particles when the missile launches to mask the fact that it just spawns out of thin air

        muzzleFlash.transform.position = spawner.transform.position;
        muzzleFlash.GetComponent<ParticleSystem>().Play();
    }
}

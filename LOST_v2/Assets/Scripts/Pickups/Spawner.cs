using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public bool spawnAtStart = true;
    public bool respawnable = false;
    public float respawnTime;
    public GameObject objectToSpawn;
    public Vector3 spawnOffset = new Vector3(0, 1, 0);
    public Vector3 gizmoSize = new Vector3(0.5f, 0.5f, 0.5f);
    public Color gizmoColor = new Color(100, 100, 100, 200);

    private GameObject spawnedObject;
    private float timeUntilNextSpawn;
    private Transform tf;

    private void Awake()
    {
        tf = gameObject.transform;
    }

    // Use this for initialization
    void Start () {
        if (spawnAtStart)
        {
            Spawn();
        }
        else
        {
            timeUntilNextSpawn = respawnTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (respawnable)
        {
            //reduces timer is object does not exist
            if (spawnedObject == null) timeUntilNextSpawn -= Time.deltaTime;

            //if timer is done, spawns new object 
            if (timeUntilNextSpawn <= 0) Spawn();
        }
    }

    public void Spawn()
    {
        //create object
        spawnedObject = Instantiate(objectToSpawn, tf.position + spawnOffset, tf.rotation);
        //reset respawn timer
        timeUntilNextSpawn = respawnTime;
    }

    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(new Vector3(0, gizmoSize.y/2, 0), gizmoSize);
        Gizmos.DrawRay(new Ray(Vector3.zero, Vector3.up));
    }
}


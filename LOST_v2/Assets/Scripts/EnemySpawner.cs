using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public float respawnTime;
    public List<GameObject> objectsToSpawn;
    private GameObject spawnedObject;
    private float timeUntilNextSpawn;
    private Transform tf;
    private Renderer render;
    public List<GameObject> objectList;
    public int maxObjects;
    public Vector3 gizmoSize;
    public Color gizmoColor;
    

    // Use this for initialization
    void Start()
    {
        int randomNum;

        tf = gameObject.transform;
        render = gameObject.GetComponent<Renderer>();
        randomNum = Random.Range(0, objectsToSpawn.Count);
        Spawn(randomNum);
    }

    // Update is called once per frame
    void Update()
    {
        int randomNum;

        //reduces timer is object does not exist
        if (objectList.Count <= maxObjects) timeUntilNextSpawn -= Time.deltaTime;

        //if timer is done, spawns new object 
        if (timeUntilNextSpawn <= 0 && objectList.Count <= maxObjects)
        {
            randomNum = Random.Range(0, objectsToSpawn.Count);
            Spawn(randomNum);
        }
    }

    public void Spawn(int itemToSpawn)
    {
        //create object
        spawnedObject = Instantiate(objectsToSpawn[itemToSpawn], tf.position + new Vector3(Random.Range(render.bounds.size.x/2 * -1, render.bounds.size.x / 2), 1, 0), tf.rotation);
        objectList.Add(spawnedObject);
        spawnedObject.GetComponent<EnemyController>().parentSpawner = this;
        //reset respawn timer
        timeUntilNextSpawn = respawnTime;
    }

    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(new Vector3(0, 0, 0), gizmoSize);
        Gizmos.DrawRay(new Ray(Vector3.zero, Vector3.up));
    }
}


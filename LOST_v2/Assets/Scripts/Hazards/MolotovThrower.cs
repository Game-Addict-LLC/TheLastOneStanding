using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MolotovThrower : MonoBehaviour
{
    public float maxVertAngle;
    public float minVertAngle;
    public float maxHorizAngle;
    public float minHorizAngle;
    public float maxSpeed;
    public float minSpeed;
    public GameObject objectToSpawn;
    public Transform spawnLocation;
    public float startDelay;
    public float maxTimeToSpawn;
    public float minTimeToSpawn;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = startDelay;
        if (spawnLocation == null)
        {
            spawnLocation = gameObject.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            timer = Random.Range(minTimeToSpawn, maxTimeToSpawn);

            GameObject spawnedObject = Instantiate(objectToSpawn, spawnLocation.position, spawnLocation.rotation);
            Quaternion tempRot = Quaternion.Euler(Random.Range(minVertAngle, maxVertAngle) * -1, Random.Range(minHorizAngle, maxHorizAngle), 0);
            float tempSpeed = Random.Range(minSpeed, maxSpeed);
            spawnedObject.GetComponent<Rigidbody>().AddForce(tempRot * spawnLocation.forward * tempSpeed);
            spawnedObject.GetComponent<Rigidbody>().AddTorque(Quaternion.Euler(0, tempRot.eulerAngles.y, 0) * spawnLocation.right * tempSpeed);

            Debug.DrawLine(spawnLocation.position, spawnLocation.position + (tempRot * spawnLocation.forward), Color.blue, 2);
        }
    }
}

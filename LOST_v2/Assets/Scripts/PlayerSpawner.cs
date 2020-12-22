using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpawner : MonoBehaviour
{

    public float respawnTime;
    public GameObject objectToSpawn;
    public List<GameObject> spawnLocations = new List<GameObject>();
    private List<Transform> spawnTransforms = new List<Transform>();
    private Transform nextLocation;
    private GameObject spawnedObject;
    private float timeUntilNextSpawn;

    public Vector3 gizmoSize;
    public Color gizmoColor;

    public int lives;
    private int livesLeft;
    public Text livesText;
    public Text respawnTimer;

    // Use this for initialization
    void Start()
    {
        livesText = GameObject.Find("Lives Text").GetComponent<Text>();
        respawnTimer = GameObject.Find("Timer Text").GetComponent<Text>();

        respawnTimer.enabled = false;
        livesLeft = lives;
        UpdateUI(livesText, "Lives: " + livesLeft);

        for (int i = 0; i < spawnLocations.Count; i++)
        {
            spawnTransforms.Add(spawnLocations[i].transform);
        }

        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        //reduces timer is object does not exist
        if (spawnedObject == null && livesLeft >= 0)
        {
            respawnTimer.enabled = true;
            UpdateUI(respawnTimer, Mathf.Ceil(timeUntilNextSpawn).ToString());
            timeUntilNextSpawn -= Time.deltaTime;
        }
        else if(livesLeft < 0)
        {
            Debug.Log("game over");
            SceneSwapper.swapper.SwapScenes("Game Over");
        }

        //if timer is done, spawns new object 
        if (timeUntilNextSpawn <= 0)
        {
            respawnTimer.enabled = false;
            livesLeft--;
            UpdateUI(livesText, "Lives: " + livesLeft);
            Spawn();
        }
    }

    public void Spawn()
    {
        nextLocation = spawnTransforms[Random.Range(0, spawnLocations.Count - 1)];

        //create object
        spawnedObject = Instantiate(objectToSpawn, nextLocation.position + new Vector3(0, 1, 0), nextLocation.rotation);
        //reset respawn timer
        timeUntilNextSpawn = respawnTime; 
    }

    public void UpdateUI(Text textToUpdate, string newText)
    {
        textToUpdate.text = newText;
    }

    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(new Vector3(0, gizmoSize.y / 2, 0), gizmoSize);
        Gizmos.DrawRay(new Ray(Vector3.zero, Vector3.up));
    }
}

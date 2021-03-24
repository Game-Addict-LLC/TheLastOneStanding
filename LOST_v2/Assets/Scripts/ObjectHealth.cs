using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHealth : Health
{
    public List<GameObject> normalModels;
    public GameObject objectToSpawn;
    public GameObject destroyedModel;
    public float destructionDelay;

    // Start is called before the first frame update
    void Start()
    {
        if (destroyedModel != null)
        {
            destroyedModel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnDeath()
    {
        Instantiate(objectToSpawn, new Vector3(gameObject.transform.position.x, gameObject.GetComponent<MeshRenderer>().bounds.min.y + 1, gameObject.transform.position.z), Quaternion.identity);

        foreach (GameObject obj in normalModels)
        {
            foreach (MeshRenderer renderer in obj.GetComponents<MeshRenderer>())
            {
                renderer.enabled = false;
            }
            foreach (Collider collider in obj.GetComponents<Collider>())
            {
                collider.enabled = false;
            }
        }

        if (destroyedModel != null)
        {
            destroyedModel.SetActive(true);
        }

        base.OnDeath(destructionDelay);
    }
}

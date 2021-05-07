using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Molotov : Hazard
{
    public GameObject damageField;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer).Contains("Player") == false) //Spawns object when bottle hits a nonplayer object
        {
            Instantiate(damageField, transform.position, Quaternion.LookRotation(collision.GetContact(0).normal));
            gameObject.GetComponent<Renderer>().enabled = false;
            gameObject.GetComponent<Collider>().enabled = false;
            Destroy(gameObject, 1); //Delays destruction to allow particle system to naturally end
        }
    }
}

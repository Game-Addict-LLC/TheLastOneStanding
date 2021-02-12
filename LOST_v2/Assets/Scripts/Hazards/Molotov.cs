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
        Instantiate(damageField, transform.position, Quaternion.LookRotation(collision.GetContact(0).normal));
        Destroy(gameObject);
    }
}

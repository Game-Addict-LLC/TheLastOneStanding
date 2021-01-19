using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killbox : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.GetComponent<Health>())
        {
            collider.gameObject.GetComponent<Health>().TakeDamage(999999);
        }
        else
        {
            Destroy(collider.gameObject);
        }
    }
}

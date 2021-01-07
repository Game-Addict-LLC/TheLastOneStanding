using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

    public float speed;
    [HideInInspector] public float damage;

	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * speed, ForceMode.VelocityChange);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision collider)
    {
        GameObject tempObject = collider.gameObject;
        if (tempObject.layer != gameObject.layer)
        {
            if (tempObject.GetComponent<Health>() != null)
            {
                tempObject.GetComponent<Health>().TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}

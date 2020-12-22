using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour {

    private Transform tf;
    public float rotateSpeed;

	// Use this for initialization
	void Start () {
        tf = GetComponent<Transform>();
	}

    // Update is called once per frame
    void Update()
    {
        Spin();
    }

    void Spin()
    {
        tf.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }

    protected virtual void OnPickup(GameObject target)
    {
        //Runs when a pickup is grabbed
        //Replaced with pickup specific function (i.e. damage, health, RoF)
        Debug.Log(target.name + " picked up " + gameObject.name);

        Destroy(gameObject);
    }

    protected void OnTriggerEnter(Collider other)
    {
        OnPickup(other.gameObject);
    }
}

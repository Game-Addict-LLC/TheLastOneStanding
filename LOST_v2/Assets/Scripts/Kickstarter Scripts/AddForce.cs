using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForce : MonoBehaviour
{
    public Vector3 forceToAdd = new Vector3(500, 1000, 0);

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(forceToAdd);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

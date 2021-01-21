using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainStretch : MonoBehaviour
{
    public Transform pointOne;
    public Transform pointTwo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Sets position between start point and current hook point
        transform.position = new Vector3((pointOne.position.x + pointTwo.position.x) / 2, transform.position.y, (pointOne.position.z + pointTwo.position.z) / 2);

        //adjusts scale so width remains constant, but length bounds are at start point and hook
        transform.localScale = new Vector3(Mathf.Sqrt(Mathf.Pow(pointOne.position.x - pointTwo.position.x, 2) + Mathf.Pow(pointOne.position.z - pointTwo.position.z, 2)) / 10, 1, 0.05f);
        //rotates chain object to align with start point
        transform.LookAt(pointOne);
        //rotates chain object by 90 degrees due to texture rotation
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 90, transform.rotation.eulerAngles.x));
        //adjusts UV scaling to maintain a constant chain link size
        transform.GetComponent<Renderer>().material.mainTextureScale = new Vector2(transform.lossyScale.x * 20, 1);
    }
}

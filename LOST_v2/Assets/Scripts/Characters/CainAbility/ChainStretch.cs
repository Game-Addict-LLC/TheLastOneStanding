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
        transform.position = new Vector3((pointOne.position.x + pointTwo.position.x) / 2, transform.position.y, (pointOne.position.z + pointTwo.position.z) / 2);
        transform.localScale = new Vector3(Mathf.Sqrt(Mathf.Pow(pointOne.position.x - pointTwo.position.x, 2) + Mathf.Pow(pointOne.position.z - pointTwo.position.z, 2)) / 10, 1, 0.05f);
        transform.LookAt(pointOne);
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 90, transform.rotation.eulerAngles.x));
        transform.GetComponent<Renderer>().material.mainTextureScale = new Vector2(transform.lossyScale.x * 20, 1);
    }
}

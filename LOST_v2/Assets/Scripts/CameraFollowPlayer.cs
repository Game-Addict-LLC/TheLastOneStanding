using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour {

    public Transform targetTf;
    public Vector3 offset;


    private Transform tf;
	// Use this for initialization
	void Start () {
        tf = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        if (targetTf)
        {
            tf.position = targetTf.position + offset;
            tf.LookAt(targetTf.position);
        }
	}

    public void Retarget()
    {
        targetTf = GameManager.instance.playerOne.gameObject.transform;
    }
}

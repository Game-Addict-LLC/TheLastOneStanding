using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour {

    public string targetPlayer;
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
        if (targetPlayer == "P1")
        {
            targetTf = GameManager.instance.playerOne.gameObject.transform;
        }
        else if (targetPlayer == "P2")
        {
            targetTf = GameManager.instance.playerTwo.gameObject.transform;
        }
        else
        {
            Debug.Log("Invalid target ID: " + targetPlayer);
        }
    }

    public void Retarget(string targetID)
    {
        if (targetID == "P1")
        {
            targetTf = GameManager.instance.playerOne.gameObject.transform;
        }
        else if (targetID == "P2")
        {
            targetTf = GameManager.instance.playerTwo.gameObject.transform;
        }
        else
        {
            Debug.Log("Invalid target ID: " + targetID);
        }
    }

    public void Retarget(Transform newTarget)
    {
        targetTf = newTarget;
    }
}

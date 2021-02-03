using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameManager.instance.pauseMenu = gameObject;
        gameObject.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UnPause()
    {
        GameManager.instance.UnPause();
    }
}

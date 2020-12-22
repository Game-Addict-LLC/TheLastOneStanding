using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwapper : MonoBehaviour {

    public static SceneSwapper swapper;

	// Use this for initialization
	void Awake () {
        if (swapper == null)
        {
            swapper = this;
        }
        else if (swapper != this)
        {
            Destroy(this);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SwapScenes(string sceneToSwapTo)
    {
        SceneManager.LoadScene(sceneToSwapTo);
    }

    public void EndApplication()
    {
        Application.Quit();
    }
}

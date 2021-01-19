using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public void CallSwapScene(string targetScene)
    {
        SceneSwapper.swapper.SwapScenes(targetScene);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}

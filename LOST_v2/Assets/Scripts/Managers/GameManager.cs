using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public bool isPaused = false;
    public Controller playerOne;
    public Controller playerTwo;
    public GameObject pauseMenu;

    public CombatUIManager combatUI;

    public float musicVolume = 1;
    public float sfxVolume = 1;

    [HideInInspector] public string winner = "Draw";
    [HideInInspector] public string winLossRecord = "0 - 0";

    private bool pausedThisFrame = false;

	// Use this for initialization
	void Awake () {
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        musicVolume = PlayerPrefs.GetFloat("Music Volume");
        sfxVolume = PlayerPrefs.GetFloat("SFX Volume");
    }
	
	// Update is called once per frame
	void Update () {
        if (pausedThisFrame)
        {
            pausedThisFrame = false;
        }
    }

    public void Pause()
    {
        if (pauseMenu)
        {
            pauseMenu.SetActive(true);
        }
        isPaused = true;
        Time.timeScale = 0.0f;
    }

    public void UnPause()
    {
        if (pauseMenu)
        {
            pauseMenu.SetActive(false);
        }
        isPaused = false;
        Time.timeScale = 1.0f;
    }

    public void TogglePause()
    {
        if (!pausedThisFrame)
        {
            pausedThisFrame = true;

            Debug.Log("Toggle pause");
            if (isPaused)
            {
                Debug.Log("unpause");
                UnPause();
            }
            else
            {
                Debug.Log("pause");
                Pause();
            }
        }
    }

    public void SetTargets()
    {
        if (playerOne != null && playerTwo != null)
        {
            playerOne.opponent = playerTwo.gameObject;
            playerTwo.opponent = playerOne.gameObject;
        }
    }
}

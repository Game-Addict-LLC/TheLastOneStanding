using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Settings : MonoBehaviour {

    public Dropdown qualitySettings;
    public Dropdown resolutionSettings;
    public Toggle fullscreenToggle;
    public Slider musicSettings;
    public Slider sfxSettings;

    public Resolution[] resolutions;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SaveOptions()
    {
        SwapQuality();
        SwapVolume();
        SwapResolution();
    }

    public void SwapQuality()
    {
        QualitySettings.SetQualityLevel(qualitySettings.value, false);
        PlayerPrefs.SetInt("Quality Settings", qualitySettings.value);
    }

    public void SwapVolume()
    {
        GameManager.instance.musicVolume = musicSettings.value;
        GameManager.instance.sfxVolume = sfxSettings.value;
        PlayerPrefs.SetFloat("Music Volume", musicSettings.value);
        PlayerPrefs.SetFloat("SFX Volume", sfxSettings.value);
    }

    public void SwapResolution()
    {
        Screen.SetResolution(resolutions[resolutionSettings.value].width, resolutions[resolutionSettings.value].height, fullscreenToggle.isOn);
        PlayerPrefs.SetInt("Resolution Width", resolutions[resolutionSettings.value].width);
        PlayerPrefs.SetInt("Resolution Height", resolutions[resolutionSettings.value].height);
        PlayerPrefs.SetInt("Fullscreen Toggle", (fullscreenToggle.isOn ? 1: 0));
    }

    public void GetData()
    {
        qualitySettings.value = QualitySettings.GetQualityLevel();
        musicSettings.value = GameManager.instance.musicVolume;
        fullscreenToggle.isOn = Screen.fullScreen;

        //Sets up resolution dropdown
        resolutions = Screen.resolutions;
        resolutionSettings.ClearOptions();
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionSettings.AddOptions(options);
        resolutionSettings.value = currentResolutionIndex;
        resolutionSettings.RefreshShownValue();
    }
}

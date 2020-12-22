using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Settings : MonoBehaviour {

    public Dropdown qualitySettings;
    public InputField horizontalResolution;
    public InputField verticalResolution;
    public Toggle fullscreenToggle;
    public Slider volumeSettings;

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
        GameManager.instance.musicVolume = volumeSettings.value;
        PlayerPrefs.SetFloat("Volume Scalar", volumeSettings.value);
    }

    public void SwapResolution()
    {
        Screen.SetResolution(int.Parse(horizontalResolution.text), int.Parse(verticalResolution.text), fullscreenToggle.isOn);
        PlayerPrefs.SetInt("Horizontal Resolution", int.Parse(horizontalResolution.text));
        PlayerPrefs.SetInt("Vertical Resolution", int.Parse(verticalResolution.text));
        PlayerPrefs.SetInt("Fullscreen Toggle", (fullscreenToggle.isOn ? 1: 0));
    }

    public void GetData()
    {
        qualitySettings.value = QualitySettings.GetQualityLevel();
        volumeSettings.value = GameManager.instance.musicVolume;
        horizontalResolution.text = Screen.width.ToString();
        verticalResolution.text = Screen.height.ToString();
        fullscreenToggle.isOn = Screen.fullScreen;
    }
}

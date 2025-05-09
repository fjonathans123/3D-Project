using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Slider SFX = null;
    [SerializeField] private Slider BGM = null;
    [SerializeField] private AudioMixer Mixer = null;

    Resolution[] resolutions;
    public Dropdown resolutionOption;

    void Start()
    {
        resolutions = Screen.resolutions;
        resolutionOption.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i =0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height);
            {
                currentResolutionIndex = i;
            }
        }
        resolutionOption.AddOptions(options);
        resolutionOption.value = currentResolutionIndex;
        resolutionOption.RefreshShownValue();

        if (PlayerPrefs.HasKey("Volume Value"))
        {
            LoadValue();
        }
        else
        {
            SetMusicVolume();
        }

        if (PlayerPrefs.HasKey("SFX Value"))
        {
            LoadSFXValue();
        }
        else
        {
            SetSFXVolume();
        }
    }

    public void SetResolution(int resoIndex)
    {
        Resolution reso = resolutions[resoIndex];
        Screen.SetResolution(reso.width, reso.height, Screen.fullScreen);
    }
    public void SetMusicVolume()
    {
        float volumeValue = BGM.value;
        Mixer.SetFloat("BGM", Mathf.Log10(volumeValue) * 20);
        PlayerPrefs.SetFloat("Volume Value", volumeValue);
    }

    void LoadValue()
    {
        float loadValue = PlayerPrefs.GetFloat("Volume Value");
        BGM.value = loadValue;
        SetMusicVolume();
    }

    public void SetSFXVolume()
    {
        float SFXValue = SFX.value;
        Mixer.SetFloat("SFX", Mathf.Log10(SFXValue) * 20);
        PlayerPrefs.SetFloat("SFX Value", SFXValue);
    }

    void LoadSFXValue()
    {
        float loadSFXValue = PlayerPrefs.GetFloat("SFX Value");
        SFX.value = loadSFXValue;
        SetSFXVolume();
    }

    public void steFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
} 
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] GameObject eventSystem;

    [SerializeField] Sprite buttonSelected;
    [SerializeField] Sprite buttonUnselected;
    bool isFullscreened;

    [SerializeField] TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;

    [SerializeField] Slider brightnessSlider;

    public static event Action CloseSettings;

    private void Start()
    {
        resolutions = Screen.resolutions;
        List<string> resolutionOptions = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            resolutionOptions.Add($"{resolutions[i].width} x {resolutions[i].height}");

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }

        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        brightnessSlider.value = 0.5f;
    }

    public void ChangeFullScreen(GameObject fullscreenButton)
    {
        if(isFullscreened)
        {
            isFullscreened = false;
            fullscreenButton.GetComponent<Image>().sprite = buttonUnselected;
        }
        else
        {
            isFullscreened = true;
            fullscreenButton.GetComponent<Image>().sprite = buttonSelected;
        }

        Screen.fullScreen = isFullscreened;
    }

    public void ChangeResolution()
    {
        Screen.SetResolution(resolutions[resolutionDropdown.value].width, resolutions[resolutionDropdown.value].height, isFullscreened);
    }

    public void ChangeBrightness()
    {
        Screen.brightness = brightnessSlider.value;
        RenderSettings.ambientLight = new Color(brightnessSlider.value, brightnessSlider.value, brightnessSlider.value, 1);
    }

    public void Back()
    {
        eventSystem.SetActive(false);
        CloseSettings?.Invoke();
        SceneManager.UnloadSceneAsync("SettingsMenu", UnloadSceneOptions.None);
    }
}

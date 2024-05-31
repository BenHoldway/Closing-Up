using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] GameObject canvas;

    [SerializeField] GameObject settingsScreen;
    [SerializeField] GameObject controlsScreen;

    [SerializeField] Sprite buttonSelected;
    [SerializeField] Sprite buttonUnselected;
    bool isFullscreened;

    public static event Action CloseSettings;

    private void Start()
    {
        canvas.SetActive(false);
        settingsScreen.SetActive(false);
        controlsScreen.SetActive(false);

        //Set game to be fullscreen
        isFullscreened = false;
        ChangeFullScreen(null);
    }

    private void OnEnable()
    {
        SceneLoader.ShowSettings += ShowSettings;
    }

    private void OnDisable()
    {
        SceneLoader.ShowSettings -= ShowSettings;
    }

    void ShowSettings()
    {
        canvas.SetActive(true);
        settingsScreen.SetActive(true);
    }

    public void HideSettings()
    {
        canvas.SetActive(false);
        settingsScreen.SetActive(false);
        CloseSettings?.Invoke();
    }

    public void ChangeFullScreen(GameObject fullscreenButton)
    {
        //Unselect button
        if(isFullscreened)
        {
            if(fullscreenButton != null)
                fullscreenButton.GetComponent<Image>().sprite = buttonUnselected;
        }
        //Select button
        else
        {
            if (fullscreenButton != null)
                fullscreenButton.GetComponent<Image>().sprite = buttonSelected;
        }

        //Change fullscreen mode
        isFullscreened = !isFullscreened;
        Screen.fullScreen = isFullscreened;
    }
}

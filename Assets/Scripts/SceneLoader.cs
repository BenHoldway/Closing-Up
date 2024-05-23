using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject eventSystem;

    // Start is called before the first frame update
    private void OnEnable()
    {
        SettingsManager.CloseSettings += UnloadSettings;
    }

    private void OnDisable()
    {
        SettingsManager.CloseSettings -= UnloadSettings;
    }

    public void StartGame()
    {
        SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Single);
    }

    public void LoadSettingsMenu()
    {
        mainMenu.SetActive(false);
        eventSystem.SetActive(false);
        SceneManager.LoadSceneAsync("SettingsMenu", LoadSceneMode.Additive);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    void UnloadSettings()
    {
        mainMenu.SetActive(true);
        eventSystem.SetActive(true);
    }
}

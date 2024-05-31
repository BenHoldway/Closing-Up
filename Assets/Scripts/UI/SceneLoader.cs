using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    const string MainMenu = "MainMenu";
    const string Settings = "SettingsMenu";
    const string Game = "GameScene";


    [SerializeField] GameObject objectsToHide;

    public static event Action ShowSettings;

    private void Start()
    {
        //Load settings when new scene is loaded if it is not already loaded
        if(!SceneManager.GetSceneByName(Settings).isLoaded)
            SceneManager.LoadSceneAsync(Settings, LoadSceneMode.Additive);
    }

    private void OnEnable()
    {
        SettingsManager.CloseSettings += UnloadSettings;
        DataHolderController.LoadNextShift += LoadGame;
    }

    private void OnDisable()
    {
        SettingsManager.CloseSettings -= UnloadSettings;
        DataHolderController.LoadNextShift -= LoadGame;
    }

    public void LoadGame()
    {
        //Unload main menu if it is loaded
        if(SceneManager.GetSceneByName(MainMenu).isLoaded)
            SceneManager.UnloadSceneAsync(MainMenu, UnloadSceneOptions.None);

        //Unload game scene if it is loaded
        //Prevents more than 1 instance of the game scene to be loaded
        if (SceneManager.GetSceneByName(Game).isLoaded)
            SceneManager.UnloadSceneAsync(Game, UnloadSceneOptions.None);

        //Loads the game scene
        SceneManager.LoadSceneAsync(Game, LoadSceneMode.Additive);
    }

    public void LoadSettingsMenu()
    {
        if (objectsToHide == null)
            return;

        //Hide all the relevant objects to show settings scene
        objectsToHide.SetActive(false);
        ShowSettings?.Invoke();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadMainMenu()
    {
        //Unload game and load main menu
        SceneManager.UnloadSceneAsync(Game, UnloadSceneOptions.None);
        SceneManager.LoadSceneAsync(MainMenu, LoadSceneMode.Additive);
    }

    void UnloadSettings()
    {
        if (objectsToHide == null)
            return;

        //Show all relevant objects again to hide settings
        objectsToHide.SetActive(true);
    }
}

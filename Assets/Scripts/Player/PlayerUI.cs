using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerUI : MonoBehaviour
{
    PlayerControls playerControls;

    [SerializeField] GameObject pauseUI;
    [SerializeField] GameObject gameGUI;

    // Start is called before the first frame update
    void Awake()
    {
        playerControls = new PlayerControls();

        pauseUI.SetActive(false);
    }

    private void OnEnable()
    {
        playerControls.Enable();

        playerControls.UI.PauseGame.performed += PauseGame;
        ShiftManager.CompleteShiftEvent += DisablePlayerUI;
    }

    private void OnDisable()
    {
        playerControls.Disable();

        playerControls.UI.PauseGame.performed -= PauseGame;
        ShiftManager.CompleteShiftEvent -= DisablePlayerUI;
    }

    //Show pause UI and pause game time when the Pause key is pressed
    void PauseGame(InputAction.CallbackContext ctx)
    {
        pauseUI.SetActive(true);
        gameGUI.SetActive(false);

        Time.timeScale = 0;
    }

    //Hide pause UI and unpause game time
    public void UnpauseGame()
    {
        pauseUI.SetActive(false);
        gameGUI.SetActive(true);

        Time.timeScale = 1;
    }

    void DisablePlayerUI()
    {
        this.enabled = false;
    }
}

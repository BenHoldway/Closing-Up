using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    PlayerControls playerControls;

    public float CurrentTime {  get; private set; }

    public static event Action UnpausedTime;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance);

        playerControls = new PlayerControls();

    }

    private void OnEnable()
    {
        playerControls.Enable();

        playerControls.UI.NextShift.performed += _ =>
        {
            Time.timeScale = 1;
            UnpausedTime?.Invoke();
        };

        ShiftManager.NextShiftEvent += ResetTime;
    }

    private void OnDisable()
    {
        playerControls.Disable();

        ShiftManager.NextShiftEvent -= ResetTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CurrentTime += Time.fixedDeltaTime;
    }

    void ResetTime()
    {
        Time.timeScale = 0;
        CurrentTime = 0;
    }
}
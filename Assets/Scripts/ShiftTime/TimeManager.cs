using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    public float CurrentTime {  get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance);

        ResetTime();
    }

    private void OnEnable()
    {
        ShiftManager.CompleteShiftEvent += PauseTime;
    }

    private void OnDisable()
    {
        ShiftManager.CompleteShiftEvent -= PauseTime;
    }

    //Increase current time with Unity time progression
    void FixedUpdate()
    {
        CurrentTime += Time.fixedDeltaTime;
    }

    void PauseTime()
    {
        Time.timeScale = 0;
    }

    void ResetTime()
    {
        Time.timeScale = 1;
        CurrentTime = 0;
    }
}

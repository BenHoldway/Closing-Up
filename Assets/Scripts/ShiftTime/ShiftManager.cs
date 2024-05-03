using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ShiftManager : MonoBehaviour
{
    public static ShiftManager Instance;

    public float ShiftStartTime {  get; private set; }
    public float ShiftEndTime {  get; private set; }

    float ShiftCurrentTime;
    int lastTimeIncrement;

    int shiftCount;

    [SerializeField] TMP_Text shiftTimeText;

    public static event Action NextShiftEvent;

    // Start is called before the first frame update
    void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(Instance);


        ShiftStartTime = 17f * 60f;
        ShiftEndTime = 23f * 60f;

        ShiftCurrentTime = ShiftStartTime;
        lastTimeIncrement = 0;

        shiftCount = 0;
        shiftTimeText.text = $"{TimeSpan.FromSeconds(ShiftStartTime).ToString(@"mm\:ss")}";

    }

    private void OnEnable()
    {
        TimeManager.UnpausedTime += NextShift;
    }

    private void OnDisable()
    {
        TimeManager.UnpausedTime -= NextShift;
    }

    // Update is called once per frame
    void Update()
    {
        if (ShiftCurrentTime >= ShiftEndTime)
            EndShift();

        int minutes = TimeSpan.FromSeconds(TimeManager.Instance.CurrentTime).Minutes;
        int seconds = TimeSpan.FromSeconds(TimeManager.Instance.CurrentTime).Seconds;

        if (minutes != 0)
            seconds += minutes * 60;

        //Increase shift time by 5, every 2 seconds
        if(lastTimeIncrement + 1 <= seconds)
        {
            ShiftCurrentTime += 5;
            lastTimeIncrement = seconds;
            shiftTimeText.text = $"{TimeSpan.FromSeconds(ShiftCurrentTime).ToString(@"mm\:ss")}";
        }
    }

    void EndShift()
    {
        NextShiftEvent?.Invoke();
        
        shiftCount++;
        ShiftCurrentTime = ShiftStartTime;
        lastTimeIncrement = 0;
    }

    void NextShift()
    {
        shiftTimeText.text = $"{TimeSpan.FromSeconds(ShiftCurrentTime).ToString(@"mm\:ss")}";
    }
}

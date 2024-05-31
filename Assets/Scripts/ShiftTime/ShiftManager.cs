using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ShiftManager : MonoBehaviour
{
    private static ShiftManager instance;
    public static ShiftManager Instance { get { return instance; } }

    public float ShiftStartTime {  get; private set; }
    public float ShiftEndTime {  get; private set; }

    public float ShiftCurrentTime { get; private set; }
    int lastTimeIncrement;

    public int ShiftCount { get; set; }

    public float DifficultyMultiplier { get; private set; }
    [SerializeField] float hardestDifficulty;

    [SerializeField] TMP_Text shiftTimeText;
    [SerializeField] GameObject shiftEndUI;
    [SerializeField] TMP_Text shiftEndText;

    public static event Action CompleteShiftEvent;
    public static event Action NextShiftEvent;
    public static event Action EndDay;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(instance);

        ShiftStartTime = 16f * 60f; //Time for 4pm
        ShiftEndTime = 23f * 60f; //Time for 11pm

        ShiftCurrentTime = ShiftStartTime;
        lastTimeIncrement = 0;

        shiftTimeText.text = $"{TimeSpan.FromSeconds(ShiftStartTime).ToString(@"mm\:ss")}";

        DifficultyMultiplier = 1.0f;

        DataHolder shuttleObj = FindObjectOfType<DataHolder>();
        if (shuttleObj == null)
            ShiftCount = 1;
        else
            ShiftCount = shuttleObj.ShiftNum;

        shiftEndUI.SetActive(false);
    }

    private void OnEnable()
    {
        FamilyConditions.ConditionsUpdated += UpdateShiftCount;
    }

    private void OnDisable()
    {
        FamilyConditions.ConditionsUpdated -= UpdateShiftCount;
    }

    // Update is called once per frame
    void Update()
    {
        //If time is over the shift end time, call complete shift method
        if (ShiftCurrentTime >= ShiftEndTime)
            CompleteShift();

        //Get minutes and seconds from current time
        int minutes = TimeSpan.FromSeconds(TimeManager.Instance.CurrentTime).Minutes;
        int seconds = TimeSpan.FromSeconds(TimeManager.Instance.CurrentTime).Seconds;

        //Get seconds from amount of minutes
        if (minutes != 0)
            seconds += minutes * 60;

        //Increase shift time by 5, every 2 seconds
        if(lastTimeIncrement + 2 <= seconds)
        {
            ShiftCurrentTime += 5;
            lastTimeIncrement = seconds;
            shiftTimeText.text = $"{TimeSpan.FromSeconds(ShiftCurrentTime).ToString(@"mm\:ss")}";
        }
    }

    //Show shift end UI (with payments etc) and notify that the shift has ended
    void CompleteShift()
    {
        ShiftCurrentTime = ShiftStartTime;
        CompleteShiftEvent?.Invoke();

        shiftEndUI.SetActive(true);
        shiftEndText.text = $"End of Shift {ShiftCount}";
    }

    //Notify that the Next Shift button has been pressed
    public void EndShift()
    {
        NextShiftEvent?.Invoke();
    }

    //Finalises the day and increases shift counter
    void UpdateShiftCount()
    {
        ShiftCount++;

        lastTimeIncrement = 0;

        //If shift count is even, then increase difficulty
        if (ShiftCount % 2 == 0)
            IncreaseMultiplier();

        EndDay?.Invoke();
    }

    //Increase difficulty
    void IncreaseMultiplier()
    {
        DifficultyMultiplier = Mathf.Clamp(DifficultyMultiplier += 0.2f, 1, hardestDifficulty);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class ShiftManager : MonoBehaviour
{
    private static ShiftManager instance;
    public static ShiftManager Instance { get { return instance; } }

    public float ShiftStartTime {  get; private set; }
    public float ShiftEndTime {  get; private set; }

    public float ShiftCurrentTime { get; private set; }
    int lastTimeIncrement;

    static int shiftCount;

    public float DifficultyMultiplier { get; private set; }
    [SerializeField] float hardestDifficulty;

    [SerializeField] TMP_Text shiftTimeText;
    [SerializeField] GameObject shiftEndUI;
    [SerializeField] TMP_Text shiftEndText;

    public static event Action CompleteShiftEvent;
    public static event Action NextShiftEvent;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(instance);


        ShiftStartTime = 22f * 60f;
        ShiftEndTime = 23f * 60f;

        ShiftCurrentTime = ShiftStartTime;
        lastTimeIncrement = 0;

        shiftTimeText.text = $"{TimeSpan.FromSeconds(ShiftStartTime).ToString(@"mm\:ss")}";

        DifficultyMultiplier = 1.0f;

        DataHolder shuttleObj = FindObjectOfType<DataHolder>();
        if (shuttleObj == null)
            shiftCount = 1;
        else
            shiftCount = shuttleObj.ShiftNum;
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
            CompleteShift();

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

    void CompleteShift()
    {
        ShiftCurrentTime = ShiftStartTime;
        CompleteShiftEvent?.Invoke();

        shiftEndUI.SetActive(true);
        shiftEndText.text = $"End of Shift {shiftCount}";

        //DataHolder.ShiftNum = shiftCount;
    }

    public void EndShift()
    {
        NextShiftEvent?.Invoke();
        
        shiftCount++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        lastTimeIncrement = 0;

        if (shiftCount % 2 == 0)
            IncreaseMultiplier();
    }

    void NextShift()
    {
        shiftTimeText.text = $"{TimeSpan.FromSeconds(ShiftCurrentTime).ToString(@"mm\:ss")}";
    }

    //Increase difficulty
    void IncreaseMultiplier()
    {
        DifficultyMultiplier = Mathf.Clamp(DifficultyMultiplier += 0.75f, 1, hardestDifficulty);
    }
}

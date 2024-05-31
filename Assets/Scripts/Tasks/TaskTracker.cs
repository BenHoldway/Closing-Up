using System;
using TMPro;
using UnityEngine;

public class TaskTracker : MonoBehaviour
{
    const int moneyReductionPerTask = 3;
    
    public static TaskTracker Instance;

    TaskInteractable[] totalTasks;
    int taskIndex;

    [SerializeField] LightSwitch[] lights;
    [SerializeField] GameObject doorParent;
    Door[] doors;

    [SerializeField] TMP_Text totalTasksText;

    public static event Action<int> ShiftReductionsEvent;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        taskIndex = 0;
    }

    private void Start()
    {
        //Get all doors
        Door[] children = doorParent.GetComponentsInChildren<Door>();
        doors = new Door[children.Length];

        for (int i = 0; i < children.Length; i++)
            doors[i] = children[i];
    }

    private void OnEnable()
    {
        RoomManager.InitTasks += InitialiseTaskSize;
        SpawnInteractables.NewTask += AddTask;
        Electronics.NewComputer += AddTask;
        TaskInteractable.TaskCompleted += RemoveTask;
        ShiftManager.CompleteShiftEvent += ShiftReductions;
    }

    private void OnDisable()
    {
        RoomManager.InitTasks -= InitialiseTaskSize;
        SpawnInteractables.NewTask -= AddTask;
        Electronics.NewComputer -= AddTask;
        TaskInteractable.TaskCompleted -= RemoveTask;
        ShiftManager.CompleteShiftEvent -= ShiftReductions;
    }

    //Set task array size
    private void InitialiseTaskSize(int numTasks)
    {
        totalTasks = new TaskInteractable[numTasks];
        totalTasksText.text = $"{totalTasks.Length}/{totalTasks.Length}";
    }

    //Add task to the array
    void AddTask(TaskInteractable newTask)
    {
        if (totalTasks[0] == null)
            taskIndex = 0;

        totalTasks[taskIndex] = newTask;
        taskIndex++;
    }

    //Removes a task at a position
    void RemoveTask(TaskInteractable task)
    {
        int pos = ReturnTaskPos(task);
        if (totalTasks[pos] == null || pos == -1) 
            return;

        totalTasks[pos] = null;

        totalTasksText.text = $"{totalTasks.Length - GetCompletedTasks()}/{totalTasks.Length}";
    }

    //Gets amount of tasks remaining, and returns the reduction value
    void ShiftReductions()
    {
        int reductions = (totalTasks.Length - GetCompletedTasks()) * moneyReductionPerTask;
        ShiftReductionsEvent?.Invoke(reductions);
    }

    //Returns how many tasks are remaining, can be used to work out shift pay
    public int GetCompletedTasks()
    {
        int completedTasks = 0;

        for (int i = 0; i < totalTasks.Length; i++) 
        {
            if (totalTasks[i] == null)
                completedTasks++;
        }

        return completedTasks;
    }

    //Runs through the doors and lights to see which ones can be given money for (doors locked and lights off)
    public int GetHiddenCompletedTasks()
    {
        int completedTasks = 0;

        for (int i = 0; i < doors.Length; i++)
        {
            if (doors[i].IsLocked)
                completedTasks++;
        }

        for(int i = 0; i < lights.Length; i++) 
        {
            if (!lights[i].LightIsOn)
                completedTasks++;
        }

        return completedTasks;
    }

    //Gets the position of the task in the array
    int ReturnTaskPos(TaskInteractable task) 
    { 
        for(int i = 0; i < totalTasks.Length; i++)
        {
            if (totalTasks[i] == task)
                return i;
        }

        return -1;
    }
}

using System;
using UnityEngine;

public class TaskTracker : MonoBehaviour
{
    public static TaskTracker Instance;

    public static TaskInteractable[] totalTasks;
    int taskIndex;

    public static event Action<TaskInteractable, int> TaskCompleted;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        taskIndex = 0;
    }

    private void OnEnable()
    {
        RoomManager.InitTasks += InitialiseTaskSize;
        TaskInteractable.NewTask += AddTask;
        TaskInteractable.TaskCompleted += RemoveTask;
    }

    private void OnDisable()
    {
        RoomManager.InitTasks -= InitialiseTaskSize;
        TaskInteractable.NewTask -= AddTask;
        TaskInteractable.TaskCompleted -= RemoveTask;
    }

    private void InitialiseTaskSize(int numTasks)
    {
        totalTasks = new TaskInteractable[numTasks];
    }

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
        if (totalTasks[pos] == null || pos == -1) return;

        totalTasks[pos] = null;

        print($"Tasks Remaining: {GetCompletedTasks()}");
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

    //Gets the position of the task in the array
    int ReturnTaskPos(TaskInteractable task) 
    { 
        for(int i = 0; i < totalTasks.Length; i++)
        {
            if (totalTasks[i] == task)
                return i;
                    //TaskCompleted?.Invoke(task, i);
        }

        return -1;
    }
}

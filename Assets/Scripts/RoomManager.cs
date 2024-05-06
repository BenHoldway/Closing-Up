using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    TaskPicker[] rooms;
    [SerializeField] List<Task> tasks = new List<Task>();
    [SerializeField] List<Task> pickedTasks = new List<Task>();

    int minTasks;
    int maxTasks;

    float taskMultiplier;
    [SerializeField] float hardestDifficulty;

    public static event Action<GameObject, List<Task>> SpawnInteractable;

    private void OnEnable()
    {
        TimeManager.UnpausedTime += InitialiseTasks;
        ShiftManager.IncreaseDifficulty += IncreaseMultiplier;
    }

    private void OnDisable()
    {
        TimeManager.UnpausedTime -= InitialiseTasks;
        ShiftManager.IncreaseDifficulty -= IncreaseMultiplier;
    }

    // Start is called before the first frame update
    void Start()
    {
        minTasks = 2; maxTasks = 4;
        taskMultiplier = 1.0f;

        //Gets all the room children and their TaskPicker component to create an array of size rooms
        TaskPicker[] children = gameObject.GetComponentsInChildren<TaskPicker>();
        rooms = new TaskPicker[children.Length];

        for (int i = 0; i < children.Length; i++)
            rooms[i] = children[i];

        InitialiseTasks();
    }


    private void InitialiseTasks()
    {
        //Runs through each room and initialises a random amount of randomy types of tasks
        for (int i = 0; i < rooms.Length; i++)
        {
            //Random size of tasks
            int numOfTasks = UnityEngine.Random.Range((int)(minTasks * taskMultiplier), (int)(maxTasks * taskMultiplier));
            rooms[0].InitArray(numOfTasks);

            for (int taskIndex = 0; taskIndex < numOfTasks; taskIndex++)
            {
                //Initialises random task type for each task
                int randNum = UnityEngine.Random.Range(0, tasks.Count);
                rooms[0].PickTask(tasks[randNum]);

                pickedTasks.Add(tasks[randNum]);

                //print($"{rooms[i].gameObject.name} - {tasks[randNum]}");
            }
            SpawnInteractable?.Invoke(rooms[0].gameObject, pickedTasks);
        }

    }

    void IncreaseMultiplier()
    {
        //Increase difficulty
        taskMultiplier = Mathf.Clamp(taskMultiplier += 0.75f, 1, hardestDifficulty);
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;

    SpawnInteractables[] rooms;
    [SerializeField] List<Task> tasks = new List<Task>();
    [SerializeField] List<Task> pickedTasks = new List<Task>();

    int minTasks;
    int maxTasks;

    public static event Action<GameObject> SpawnInteractables;
    public static event Action<int> InitTasks;

    private void OnEnable()
    {
        TimeManager.UnpausedTime += InitialiseTasks;
    }

    private void OnDisable()
    {
        TimeManager.UnpausedTime -= InitialiseTasks;
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance);

        minTasks = 2; maxTasks = 4;

        //Gets all the room children and their TaskPicker component to create an array of size rooms
        SpawnInteractables[] children = gameObject.GetComponentsInChildren<SpawnInteractables>();
        rooms = new SpawnInteractables[children.Length];

        for (int i = 0; i < children.Length; i++)
            rooms[i] = children[i];

        InitialiseTasks();
    }


    private void InitialiseTasks()
    {
        int totalTaskNum = 0;
        float taskMultiplier = ShiftManager.DifficultyMultiplier;

        //Runs through each room and initialises a random amount of randomy types of tasks
        for (int i = 0; i < rooms.Length; i++)
        {
            //Random size of tasks
            int numOfTasks = UnityEngine.Random.Range((int)(minTasks * taskMultiplier), (int)(maxTasks * taskMultiplier));
            rooms[i].InitArray(numOfTasks);
            totalTaskNum += numOfTasks;

            for (int taskIndex = 0; taskIndex < numOfTasks; taskIndex++)
            {
                //Initialises random task type for each task
                int randNum = UnityEngine.Random.Range(0, tasks.Count);
                rooms[i].PickTask(tasks[randNum]);
            }
            SpawnInteractables?.Invoke(rooms[i].gameObject);
        }

        InitTasks?.Invoke(totalTaskNum);
    }
}

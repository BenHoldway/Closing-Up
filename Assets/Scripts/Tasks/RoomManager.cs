using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    const int MinTasksPerRoom = 2;
    const int MaxTasksPerRoom = 4;
    const int MinComputersOn = 3;
    const int MaxComputersOn = 5;

    SpawnInteractables[] rooms;
    [SerializeField] List<Task> tasks = new List<Task>();

    [SerializeField] Transform computersParentObj;

    public static event Action<GameObject> SpawnInteractable;
    public static event Action<int> InitTasks;
    public static event Action<int> ComputersInitialised;

    // Start is called before the first frame update
    void Awake()
    {
        //Gets all the room children and their TaskPicker component to create an array of size rooms
        SpawnInteractables[] children = gameObject.GetComponentsInChildren<SpawnInteractables>();
        rooms = new SpawnInteractables[children.Length];

        for (int i = 0; i < children.Length; i++)
            rooms[i] = children[i];
    }

    private void Start()
    {
        InitialiseTasks();
    }

    private void InitialiseTasks()
    {
        int totalTaskNum = 0;
        float taskMultiplier = ShiftManager.Instance.DifficultyMultiplier;

        //Runs through each room and initialises a random amount of randomy types of tasks
        for (int i = 0; i < rooms.Length; i++)
        {
            //Random size of tasks
            int numOfTasks = UnityEngine.Random.Range((int)(MinTasksPerRoom * taskMultiplier), (int)(MaxTasksPerRoom * taskMultiplier));
            rooms[i].InitArray(numOfTasks);
            totalTaskNum += numOfTasks;

            for (int taskIndex = 0; taskIndex < numOfTasks; taskIndex++)
            {
                //Initialises random task type for each task
                int randNum = UnityEngine.Random.Range(0, tasks.Count);
                rooms[i].AddTask(tasks[randNum]);
            }
        }

        int numOfComputers = UnityEngine.Random.Range((int)(MinComputersOn * taskMultiplier), (int)(MaxComputersOn * taskMultiplier));

        //Initialise the total task array
        InitTasks?.Invoke(totalTaskNum + numOfComputers);
        
        InitialiseComputers(numOfComputers, 0);

        //Spawns the tasks for each room
        foreach (SpawnInteractables room in rooms)
        {
            SpawnInteractable?.Invoke(room.gameObject);
        }
    }

    void InitialiseComputers(int totalComputers, int computersTurnedOn)
    {
        //Get each electronics component from computer objects
        Electronics[] computers = computersParentObj.GetComponentsInChildren<Electronics>();

        for(int i = 0; i < computers.Length; i++)
        {
            //Break if turned on counter is equal to total computers needed to be turned on
            if (computersTurnedOn >= totalComputers)
                break;

            //Get a random computer. If currently off, turn it on. Increase turned on counter
            int randComputer = UnityEngine.Random.Range(0, computers.Length);
            if (!computers[randComputer].IsOn)
            {
                computers[randComputer].IsOn = true;
                computersTurnedOn++;
            }
        }

        //Fail safe just in case not enough computers are turned on. Should not happen though
        if(computersTurnedOn < totalComputers)
            InitialiseComputers(totalComputers, computersTurnedOn);

        //Notify that all computers have been initialised
        ComputersInitialised?.Invoke(totalComputers);
    }
}

using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnInteractables : MonoBehaviour
{
    [SerializeField] Tilemap floor;
    [SerializeField] Transform center;

    [SerializeField] GameObject[] taskInteractable;

    [SerializeField] GameObject spawnPointParent;
    [SerializeField] Transform[] spawnPoints;

    Vector2 spawnPoint;

    Task[] tasks;
    int taskIndex;

    bool isGettingNewPos;

    public static event Action<TaskInteractable> NewTask;
    public static event Action<Vector2, GameObject, Tilemap, Transform> InteractableSetUp;

    // Start is called before the first frame update
    void Awake()
    {
        taskIndex = 0;
        spawnPoints = spawnPointParent.GetComponentsInChildren<Transform>();
    }

    private void OnEnable()
    {
        RoomManager.SpawnInteractable += StartSpawning;
    }

    private void OnDisable()
    {
        RoomManager.SpawnInteractable -= StartSpawning;
    }

    //Initialises the task array
    public void InitArray(int numOfTasks)
    {
        tasks = new Task[numOfTasks];
    }

    //Constructs the tasks  
    public void AddTask(Task task)
    {
        if (tasks == null)
            return;

        tasks[taskIndex] = task;
        taskIndex++;
    }

    private void Update()
    {
        if(isGettingNewPos)
        {
            if(ValidPoint())
            {
                isGettingNewPos = false;
                Spawn();
            }
        }
    }

    //Starts spawning interactables if room from event is this room
    void StartSpawning(GameObject _room) 
    {
        if(_room == this.gameObject)
        {
            taskIndex = 0;
            isGettingNewPos = true;
        }
    }

    void Spawn()
    {
        foreach (GameObject taskObj in taskInteractable)
        {
            //If taskObj scriptable object equals to specific task, spawn that object
            if (taskObj.GetComponent<TaskInteractable>().TaskSO == tasks[taskIndex])
            {
                //Instantiates the task interactable object and sets the position
                GameObject interactable = Instantiate(taskObj, gameObject.transform);

                InteractableSetUp?.Invoke(spawnPoint, interactable, floor, center);

                NewTask?.Invoke(interactable.GetComponent<TaskInteractable>());
                break;
            }
        }

        taskIndex++;
        if (taskIndex >= tasks.Length)
            isGettingNewPos = false;
        else
            isGettingNewPos = true;
    }

    bool ValidPoint()
    {
        //Get random transform point
        int randPos = UnityEngine.Random.Range(1, spawnPoints.Length);

        if (spawnPoints[randPos] != null && spawnPoints[randPos].gameObject.activeSelf)
        {
            spawnPoint = spawnPoints[randPos].transform.position;
            spawnPoints[randPos].gameObject.SetActive(false);
            return true;
        }

        return false;
    }
}
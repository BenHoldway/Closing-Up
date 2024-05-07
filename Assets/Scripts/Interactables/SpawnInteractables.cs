using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnInteractables : MonoBehaviour
{
    [SerializeField] Tilemap floor;
    [SerializeField] LayerMask floorLayer;
    [SerializeField] Transform center;

    [SerializeField] GameObject[] taskInteractable;

    Collider2D[] cols = new Collider2D[1];

    Vector2 spawnPoint = Vector2.zero;
    bool isAttemptingToSpawn;
    bool hasSpawned;

    Task[] tasks;
    int taskIndex;

    // Start is called before the first frame update
    void Awake()
    {
        isAttemptingToSpawn = false;
        hasSpawned = false;
        taskIndex = 0;
    }

    private void OnEnable()
    {
        RoomManager.SpawnInteractable += SpawnInteractable;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttemptingToSpawn)
        {
            if (!hasSpawned)
                hasSpawned = Spawn();

            if (hasSpawned)
                SetInteractablePos();        
        }
        
    }

    //Initialises the task array
    public void InitArray(int numOfTasks)
    {
        tasks = new Task[numOfTasks];
    }

    //Constructs the task
    public void PickTask(Task task)
    {
        if (tasks == null)
            return;

        tasks[taskIndex] = task;
        taskIndex++;
    }

    void SpawnInteractable(GameObject _room) 
    {
        //print($"Room: {room.name}, Task: {_task.name}");
        if(_room == gameObject)
        {
            //print($"Found room for {_task.name} task");
            isAttemptingToSpawn = true;
            taskIndex = 0;
        }
    }

    public void SetInteractablePos()
    {
        foreach (GameObject taskObj in taskInteractable)
        {
            //If taskObj scriptable object equals to specific task, spawn that object
            if(taskObj.GetComponent<TaskInteractable>().TaskSO == tasks[taskIndex])
            {
                //Instantiates the task interactable object and sets the position
                GameObject interactable = Instantiate(taskObj, gameObject.transform);
                interactable.transform.position = new Vector3(spawnPoint.x, spawnPoint.y, 0);
                break;
            }
        }

        taskIndex++;

        //If all tasks have been instantiated, end instantiating
        if (taskIndex >= tasks.Length)
            isAttemptingToSpawn = false;

        hasSpawned = false;
    }

    bool Spawn() 
    { 
        //Gets the random point on tilemap
        spawnPoint = GetRandomPoint();

        //Get the number of colliders with the selected layer mask that spawnPoint collides with
        //Then stores all of the colliders in cols
        int numFound = Physics2D.OverlapCircleNonAlloc(spawnPoint, 0.1f, cols, floorLayer);

        print($"{gameObject.name}: Spawn: {spawnPoint}, numFound: {numFound}");

        //Returns true if tilemap colliders are found, false if none have
        if (numFound == 0 && cols[0].gameObject.transform.IsChildOf(gameObject.transform))
            return false;
        else
            return true;
    }

    Vector2 GetRandomPoint()
    {
        //Gets the bounds of the tilemap, and the center, then sets the center to world pos
        Bounds bound = floor.GetComponent<CompositeCollider2D>().bounds;
        //Vector3 center = floor.GetComponent<CompositeCollider2D>().transform.position;

        //Creates an x and y pos from a range within the tilemap bounds
        float spawnX = Random.Range(center.position.x - bound.extents.x + 1f, center.position.x + bound.extents.x - 1f);
        float spawnY = Random.Range(center.position.y - bound.extents.y + 1f, center.position.y + bound.extents.y - 1f);

        //Returns these co-ordinates
        return new Vector2(spawnX, spawnY); 
    }
}

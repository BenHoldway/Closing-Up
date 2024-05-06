using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnInteractables : MonoBehaviour
{
    [SerializeField] Tilemap floor;

    [SerializeField] LayerMask floorLayer;
    [SerializeField] GameObject[] taskInteractable;

    Collider2D[] cols = new Collider2D[1];

    Vector2 spawnPoint = Vector2.zero;
    bool isAttemptingToSpawn;
    bool hasSpawned;

    List<Task> task = new List<Task>();
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

    void SpawnInteractable(GameObject room, List<Task> _task) 
    {
        //print($"Room: {room.name}, Task: {_task.name}");
        if(room == gameObject)
        {
            //print($"Found room for {_task.name} task");
            isAttemptingToSpawn = true;
            task = _task;
        }
    }

    public void SetInteractablePos()
    {
        foreach (GameObject taskObj in taskInteractable)
        {
            //If taskObj scriptable object equals to specific task, spawn that object
            if(taskObj.GetComponent<TaskInteractable>().TaskSO == task[taskIndex])
            {
                //Instantiates the task interactable object and sets the position
                GameObject interactable = Instantiate(taskObj);
                interactable.transform.position = new Vector3(spawnPoint.x, spawnPoint.y, 0);
                break;
            }
        }

        taskIndex++;

        //If all tasks have been instantiated, end instantiating
        if (taskIndex >= task.Count)
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
        if (numFound == 0)
            return false;
        else
            return true;
    }

    Vector2 GetRandomPoint()
    {
        //Gets the bounds of the tilemap, and the center, then sets the center to world pos
        Bounds bound = floor.GetComponent<CompositeCollider2D>().bounds;
        Vector3 center = transform.position + floor.cellBounds.center;

        //Creates an x and y pos from a range within the tilemap bounds
        float spawnX = Random.Range(center.x - bound.extents.x, center.x + bound.extents.x);
        float spawnY = Random.Range(center.y - bound.extents.y, center.y + bound.extents.y);

        //Returns these co-ordinates
        return new Vector2(spawnX, spawnY); 
    }

}

using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnInteractables : MonoBehaviour
{
    [SerializeField] Tilemap floor;
    [SerializeField] LayerMask floorLayer;
    [SerializeField] GameObject taskInteractable;

    Collider2D[] cols = new Collider2D[1];

    Vector2 spawnPoint = Vector2.zero;
    bool isAttemptingToSpawn;

    // Start is called before the first frame update
    void Awake()
    {
        isAttemptingToSpawn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || isAttemptingToSpawn)
        {
            bool hasSpawned = false;
            isAttemptingToSpawn = true;

            if (!hasSpawned)
                hasSpawned = Spawn();

            if (hasSpawned)
                SetInteractablePos();
        }
    }

    bool Spawn() 
    { 
        spawnPoint = GetRandomPoint();

        int numFound = Physics2D.OverlapCircleNonAlloc(spawnPoint, 0.01f, cols, floorLayer);

        if (numFound == 0)
            return false;
        else
            return true;
    }

    Vector2 GetRandomPoint()
    {
        Bounds bound = floor.GetComponent<CompositeCollider2D>().bounds;


        float spawnX = Random.Range(floor.transform.position.x - bound.extents.x, floor.transform.position.x + bound.extents.x);
        float spawnY = Random.Range(floor.transform.position.y - bound.extents.y, floor.transform.position.y + bound.extents.y);
        print($"X: {Random.Range(floor.transform.position.x - bound.extents.x, floor.transform.position.x + bound.extents.x)}, Y: {Random.Range(floor.transform.position.y - bound.extents.y, floor.transform.position.y + bound.extents.y)}");
        print($"Spawnpoint: {new Vector2(spawnX, spawnY)}");

        return new Vector2(spawnX, spawnY); 
    }

    void SetInteractablePos()
    {
        GameObject interactable = Instantiate(taskInteractable);
        interactable.transform.position = new Vector3(spawnPoint.x, spawnPoint.y, 0);

        isAttemptingToSpawn = false;
    }
}

using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnInteractables : MonoBehaviour
{
    [SerializeField] Tilemap floor;
    [SerializeField] LayerMask floorLayer;
    [SerializeField] GameObject taskInteractable;

    Collider2D[] cols = new Collider2D[1];

    // Start is called before the first frame update
    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonUp(0)) 
        {
            Spawn();
        }
    }

    void Spawn() 
    { 
        Vector2 spawnPoint = GetRandomPoint();

        int numFound = Physics2D.OverlapCircleNonAlloc(spawnPoint, 0.01f, cols, floorLayer);

        if (numFound == 0)
            Spawn();
        else
        {
            GameObject interactable = Instantiate(taskInteractable);
            interactable.transform.position = new Vector3(spawnPoint.x, spawnPoint.y, 0);
        }
    }

    Vector2 GetRandomPoint()
    {
        Bounds bound = floor.GetComponent<CompositeCollider2D>().bounds;

        float spawnX = Random.Range(-bound.extents.x, bound.extents.x);
        float spawnY = Random.Range(-bound.extents.y, bound.extents.y);

        return new Vector2(spawnX, spawnY); 
    }
}

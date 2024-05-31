using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChangePosition : MonoBehaviour
{
    [SerializeField] LayerMask floorLayer;
    [SerializeField] LayerMask spawnableLayers;
    Tilemap floor;
    Transform center;

    Collider2D[] cols;

    Vector2 spawnPoint = Vector2.zero;

    private void OnEnable()
    {
        SpawnInteractables.InteractableSetUp += SetUp;
    }

    private void OnDisable()
    {
        SpawnInteractables.InteractableSetUp -= SetUp;
    }

    //Passes the needed variables from being spawned into this class
    public void SetUp(Vector2 _spawnPos, GameObject obj, Tilemap _floor, Transform _center)
    {
        if (obj != gameObject)
            return;

        floor = _floor;
        center = _center;

        transform.position = _spawnPos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(new Vector3(spawnPoint.x, spawnPoint.y, -10f), new Vector3(1f, 1f, Mathf.Infinity));
    }
}
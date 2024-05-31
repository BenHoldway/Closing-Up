using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFocus : MonoBehaviour
{
    Vector3 targetPos;
    [SerializeField] Transform player;

    Rigidbody2D rbPlayer;

    void Start()
    {
        transform.position = player.position;
        rbPlayer = player.gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //Get mouse position
        targetPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        //Clamp between player position, and 1 unit away
        float disX = Mathf.Clamp(targetPos.x - player.position.x, -1, 1);
        float disY = Mathf.Clamp(targetPos.y - player.position.y, -1, 1);

        //Add the velocity to position to prevent camera from lagging behind player when they move
        Vector3 finalPos = new Vector3(disX + rbPlayer.velocity.x, disY + rbPlayer.velocity.y, -10) + player.position;

        //Slowly move camera position between current position and final position
        transform.position = Vector3.Lerp(transform.position, finalPos, Time.deltaTime * 2);
    }
}

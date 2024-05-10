using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFocus : MonoBehaviour
{
    Vector3 targetPos;
    [SerializeField] Transform player;

    Rigidbody2D rbPlayer;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = player.position;
        rbPlayer = player.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        targetPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        float disX = Mathf.Clamp(targetPos.x - player.position.x, -1, 1);
        float disY = Mathf.Clamp(targetPos.y - player.position.y, -1, 1);

        Vector3 finalPos = new Vector3(disX + rbPlayer.velocity.x, disY + rbPlayer.velocity.y, -10) + player.position;

        transform.position = Vector3.Lerp(transform.position, finalPos, Time.deltaTime * 2);
    }
}

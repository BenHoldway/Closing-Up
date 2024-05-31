using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    PlayerControls playerControls;

    Rigidbody2D rb;

    [SerializeField] float speed;
    Vector2 movement;
    bool isFacingRight;

    private void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();

        isFacingRight = true;
    }

    private void OnEnable()
    {
        playerControls.Enable();

        playerControls.Player.Move.performed += ctx => { movement = ctx.ReadValue<Vector2>(); };
        playerControls.Player.Move.canceled += ctx => { movement = Vector2.zero; };
    }

    private void OnDisable()
    {
        playerControls.Disable();
        movement = Vector2.zero;
        rb.velocity = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        //Move player from input
        rb.velocity = movement * speed;

        //If movement is opposite to the way the player is facing, flip the player
        if ((movement.x > 0 && !isFacingRight) || (movement.x < 0 && isFacingRight))
            Flip();
    }

    void Flip()
    {
        //Get x local scale and flip it, flipping the player
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}

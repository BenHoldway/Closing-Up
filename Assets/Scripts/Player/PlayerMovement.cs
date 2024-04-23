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
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = movement * speed;

        if ((movement.x > 0 && !isFacingRight) || (movement.x < 0 && isFacingRight))
            Flip();
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}

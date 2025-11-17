using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 7f;
    public float jumpForce = 12f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Model Flip")]
    public Transform model;   // <- Add this and assign "Rig" in Inspector

    private Rigidbody2D rb;
    private float moveInput;
    public bool isGrounded;

    private PlayerAnimator anim;

    // Door interaction variables
    private bool isAgainstDoor = false;
    private bool isDoorOpen = false;
    private float doorDirection = 0f; // +1 = door is right, -1 = door is left

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<PlayerAnimator>();
    }

    private void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        // Flip player model based on movement direction
        if (moveInput > 0)
        {
            model.localScale = new Vector3(1, 1, 1);   // Face right
        }
        else if (moveInput < 0)
        {
            model.localScale = new Vector3(-1, 1, 1);  // Face left
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        anim?.UpdateStates(
            moveInput,
            rb.linearVelocity.y,
            isGrounded,
            isAgainstDoor,
            isDoorOpen
        );
    }

    private void FixedUpdate()
    {
        // If touching a closed door, only block movement TOWARDS the door
        if (isAgainstDoor && !isDoorOpen)
        {
            // Moving toward door (right)
            if (moveInput > 0 && doorDirection > 0)
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                return;
            }

            // Moving toward door (left)
            if (moveInput < 0 && doorDirection < 0)
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                return;
            }

            // Allowed to move away from the door
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }
        else
        {
            // Normal movement
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    // Called by Door.cs
    public void SetDoorInteraction(bool touchingDoor, bool doorIsOpen, float doorX)
    {
        isAgainstDoor = touchingDoor;
        isDoorOpen = doorIsOpen;
    
        if (touchingDoor)
            doorDirection = Mathf.Sign(doorX - transform.position.x);
        else
            doorDirection = 0f;
    }

}

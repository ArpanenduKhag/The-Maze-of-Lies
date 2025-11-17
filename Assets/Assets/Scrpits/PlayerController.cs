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
    public Transform model;   // Assign your "Rig" here in Inspector

    private Rigidbody2D rb;
    private float moveInput;
    public bool isGrounded;

    private PlayerAnimator anim;

    // Door interaction variables
    private bool isAgainstDoor = false;
    private bool isDoorOpen = false;
    private float doorDirection = 0f; // +1 = door is right, -1 = door is left


    // -----------------------------
    //       DEATH SYSTEM
    // -----------------------------
    [Header("Death & Ghost")]
    public GameObject ghostPrefab;
    private bool isDead = false;


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

        if (Input.GetButtonDown("Jump") && isGrounded && !isDead)
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
        if (isDead) 
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // If touching a CLOSED door, only block movement TOWARDS the door
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


    // ---------------------------------------------------------
    // DOOR INTERACTION (Called from Door.cs)
    // ---------------------------------------------------------
    public void SetDoorInteraction(bool touchingDoor, bool doorIsOpen, float doorX)
    {
        isAgainstDoor = touchingDoor;
        isDoorOpen = doorIsOpen;

        if (touchingDoor)
            doorDirection = Mathf.Sign(doorX - transform.position.x);
        else
            doorDirection = 0f;
    }


    // ---------------------------------------------------------
    //                     DEATH SYSTEM
    // ---------------------------------------------------------
    public void Die()
    {
        if (isDead) return; // prevent double death
        isDead = true;

        // Spawn ghost at death position
        if (ghostPrefab != null)
            Instantiate(ghostPrefab, transform.position, Quaternion.identity);

        // Disable movement temporarily
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        GetComponent<Collider2D>().enabled = false;

        // Respawn shortly
        Invoke(nameof(Respawn), 0.15f);
    }


    private void Respawn()
    {
        // Move player to spawn point
        transform.position = LevelManager.Instance.spawnPoint.position;

        // Restore movement
        rb.bodyType = RigidbodyType2D.Dynamic;
        GetComponent<Collider2D>().enabled = true;

        isDead = false;
    }
}

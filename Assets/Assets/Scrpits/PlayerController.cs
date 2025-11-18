using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 7f;
    public float jumpForce = 12f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Model Flip")]
    public Transform model;

    private Rigidbody2D rb;
    private float moveInput;
    private bool isGrounded;
    private PlayerAnimator anim;


    // ---------- DOOR INTERACTION ----------
    private bool isAgainstDoor = false;
    private bool isDoorOpen = false;
    private float doorDirection = 0f;


    // ---------- LIVES / DEATH ----------
    [Header("Death / Lives")]
    public GameObject ghostPrefab;
    private bool isDead = false;

    public int maxLives = 3;
    public int currentLives = 3;


    // ---------- SLASH ATTACK ----------
    [Header("Slash Attack")]
    public GameObject slashPrefab;      // damage hitbox
    public GameObject slashVFX;         // explosion slash particle VFX
    public float slashCooldown = 0.3f;
    private bool canSlash = true;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<PlayerAnimator>();
    }


    private void Update()
    {
        if (isDead) return;

        float mobile = MobileInput.Instance != null ? MobileInput.Instance.horizontal : 0;
        float keyboard = Input.GetAxisRaw("Horizontal");
        moveInput = Mathf.Abs(keyboard) > 0 ? keyboard : mobile;

        // Flip visual model
        if (moveInput > 0)
            model.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0)
            model.localScale = new Vector3(-1, 1, 1);

        // Jump
        if ((Input.GetButtonDown("Jump") || MobileInput.Instance.jumpPressed) && isGrounded)
            Jump();

        // Slash
        if ((Input.GetKeyDown(KeyCode.X) || MobileInput.Instance.slashPressed) && canSlash)
            Slash();

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

        // Door blocking
        if (isAgainstDoor && !isDoorOpen)
        {
            bool pushingRight = moveInput > 0 && doorDirection > 0;
            bool pushingLeft = moveInput < 0 && doorDirection < 0;

            if (pushingRight || pushingLeft)
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                return;
            }
        }

        // Normal movement
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
    }


    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }


    // ---------- SLASH ----------
    private void Slash()
    {
        if (!canSlash) return;
        canSlash = false;

        // ----------------------------------
        // SPAWN SLASH HITBOX (damage)
        // ----------------------------------
        Vector3 hitboxPos = transform.position +
                            new Vector3(model.localScale.x * 0.6f, 0, 0);

        GameObject hitbox = Instantiate(slashPrefab, hitboxPos, Quaternion.identity);

        Vector3 hitScale = hitbox.transform.localScale;
        hitScale.x = model.localScale.x;
        hitbox.transform.localScale = hitScale;


        // ----------------------------------
        // SPAWN SLASH VFX (particles only)
        // ----------------------------------
        if (slashVFX != null)
        {
            Vector3 vfxPos = transform.position +
                             new Vector3(model.localScale.x * 0.4f, 0, 0);

            GameObject vfx = Instantiate(slashVFX, vfxPos, Quaternion.identity);

            if (model.localScale.x < 0)
                vfx.transform.rotation = Quaternion.Euler(0, 180, 0);

            Destroy(vfx, 1.5f);
        }

        Invoke(nameof(ResetSlash), slashCooldown);
    }

    private void ResetSlash()
    {
        canSlash = true;
    }


    // ---------- DOOR ----------
    public void SetDoorInteraction(bool touchingDoor, bool doorIsOpen, float doorX)
    {
        isAgainstDoor = touchingDoor;
        isDoorOpen = doorIsOpen;

        if (touchingDoor)
            doorDirection = Mathf.Sign(doorX - transform.position.x);
        else
            doorDirection = 0f;
    }


    // ---------- DEATH ----------
    public void Die()
    {
        if (isDead) return;
        isDead = true;

        currentLives--;

        // SPAWN NEW GHOST & COUNT IT
        if (ghostPrefab != null)
        {
            Instantiate(ghostPrefab, transform.position, Quaternion.identity);
            GhostManager_Level3.Instance.GhostSpawned();
        }

        if (currentLives <= 0)
        {
            Debug.Log("GAME OVER!");
            LevelManager.Instance.ResetLevel();
            return;
        }

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        GetComponent<Collider2D>().enabled = false;

        Invoke(nameof(Respawn), 0.15f);
    }

    private void Respawn()
    {
        transform.position = LevelManager.Instance.spawnPoint.position;

        rb.bodyType = RigidbodyType2D.Dynamic;
        GetComponent<Collider2D>().enabled = true;

        isDead = false;
    }
}

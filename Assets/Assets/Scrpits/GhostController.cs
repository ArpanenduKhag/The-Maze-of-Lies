using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GhostController : MonoBehaviour
{
    public float speed = 2f;
    private int direction = 1;

    private Rigidbody2D rb;
    private Animator anim;

    private bool isAttacking = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 2f;
        rb.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        if (!isAttacking)
        {
            rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
        }

        // Update moving state
        bool moving = Mathf.Abs(rb.linearVelocity.x) > 0.1f;
        anim.SetBool("isMoving", moving);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 normal = collision.contacts[0].normal;

        if (normal.x < -0.5f)
        {
            direction = -1;
            Flip();
        }
        else if (normal.x > 0.5f)
        {
            direction = 1;
            Flip();
        }
    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponentInParent<PlayerController>();
        if (player != null)
        {
            StartAttack();
            player.Die();
        }
    }

    private void StartAttack()
    {
        isAttacking = true;
        anim.SetBool("isAttacking", true);

        // Stop walking
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        // Return to walk after attack animation
        Invoke(nameof(StopAttack), 0.25f);
    }

    private void StopAttack()
    {
        isAttacking = false;
        anim.SetBool("isAttacking", false);
    }
}

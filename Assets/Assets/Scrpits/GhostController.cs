using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GhostController : MonoBehaviour
{
    public float speed = 2f;
    public int direction = 1;

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

        // moving animation
        anim.SetBool("isMoving", Mathf.Abs(rb.linearVelocity.x) > 0.1f);
    }

    // Called by the front sensor
    public void FlipDirection()
    {
        direction *= -1;
        Flip();
    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;
    }

    // Called by the sensor when hitting the player
    public void AttackPlayer(PlayerController player)
    {
        if (isAttacking) return;

        isAttacking = true;
        anim.SetBool("isAttacking", true);

        // stop moving
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        // kill player
        player.Die();

        // return to walking
        Invoke(nameof(StopAttack), 0.3f);
    }

    private void StopAttack()
    {
        isAttacking = false;
        anim.SetBool("isAttacking", false);
    }
}

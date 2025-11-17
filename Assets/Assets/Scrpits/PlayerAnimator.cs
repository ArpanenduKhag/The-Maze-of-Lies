using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator animator;

    private bool attackTriggered = false;

    public void UpdateStates(
        float moveInput,
        float verticalVelocity,
        bool grounded,
        bool againstDoor,
        bool doorOpen
    )
    {
        animator.SetFloat("speed", Mathf.Abs(moveInput));
        animator.SetFloat("verticalVelocity", verticalVelocity);
        animator.SetBool("isGrounded", grounded);

        animator.SetBool("isAgainstDoor", againstDoor);
        animator.SetBool("isDoorOpen", doorOpen);

        // --- ZOMBIE ATTACK LOGIC (for pushing closed door) ---
        if (againstDoor && !doorOpen && grounded && Mathf.Abs(moveInput) > 0.1f)
        {
            if (!attackTriggered)
            {
                animator.SetTrigger("ZombieAttack");
                attackTriggered = true;
            }
        }
        else
        {
            attackTriggered = false;
        }
    }

    // ⭐ NEW: Called by PlayerController to play slash attack animation
    public void TriggerSlash()
    {
        animator.SetTrigger("Slash");
    }
}

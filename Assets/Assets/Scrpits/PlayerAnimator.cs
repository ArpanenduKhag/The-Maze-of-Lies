using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator animator;

    private bool attackTriggered = false;

    public void UpdateStates(float moveInput, float verticalVelocity, bool grounded, bool againstDoor, bool doorOpen)
    {
        animator.SetFloat("speed", Mathf.Abs(moveInput));
        animator.SetFloat("verticalVelocity", verticalVelocity);
        animator.SetBool("isGrounded", grounded);

        animator.SetBool("isAgainstDoor", againstDoor);
        animator.SetBool("isDoorOpen", doorOpen);

        // --- ZOMBIE ATTACK LOGIC ---
        // Trigger attack ONCE when pushing a closed door
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
            // Reset trigger lock once player stops pushing
            attackTriggered = false;
        }
    }
}

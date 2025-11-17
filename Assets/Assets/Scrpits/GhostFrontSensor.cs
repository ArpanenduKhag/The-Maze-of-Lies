using UnityEngine;

public class GhostFrontSensor : MonoBehaviour
{
    private GhostController ghost;

    private void Awake()
    {
        ghost = GetComponentInParent<GhostController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kill player
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
                ghost.AttackPlayer(player);

            return;
        }

        // Flip on ANY collision:
        // ground, spikes, walls, platforms, other ghosts
        if (!other.isTrigger)
        {
            ghost.FlipDirection();
        }
    }
}

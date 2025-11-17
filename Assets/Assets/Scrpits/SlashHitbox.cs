using UnityEngine;

public class SlashHitbox : MonoBehaviour
{
    public float life = 0.12f; // short clean hitbox window

    private void Start()
    {
        Destroy(gameObject, life);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ghost"))
        {
            GhostCombatController ghost = other.GetComponent<GhostCombatController>();
            if (ghost != null)
                ghost.Die();
        }
    }
}

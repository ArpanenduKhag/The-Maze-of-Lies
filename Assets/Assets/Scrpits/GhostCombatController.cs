using UnityEngine;

public class GhostCombatController : MonoBehaviour
{
    private bool isDead = false;

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        GhostManager_Level3.Instance.GhostKilled();

        Destroy(gameObject);
    }
}

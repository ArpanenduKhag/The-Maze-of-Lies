using UnityEngine;

public class Spike : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision == null || collision.collider == null) return;

        GameObject otherObj = collision.collider.gameObject;

        if (!otherObj.CompareTag("Player")) return;

        PlayerController player = otherObj.GetComponent<PlayerController>();
        if (player != null)
        {
            player.Die();
        }
        else
        {
            Debug.LogWarning($"Spike collision with Player-tagged object but no PlayerController found on {otherObj.name}", otherObj);
        }
    }
}

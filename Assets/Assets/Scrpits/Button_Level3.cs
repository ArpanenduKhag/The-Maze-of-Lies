using UnityEngine;

public class Button_Level3 : MonoBehaviour
{
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;
            GhostManager_Level3.Instance.ButtonPressed();
        }
    }
}

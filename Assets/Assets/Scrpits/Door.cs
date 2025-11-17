using UnityEngine;

public class Door : MonoBehaviour
{
    public bool doorOpen = false;

    private BoxCollider2D col;

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        col.isTrigger = false; // Door is solid at start
    }

    // Player enters trigger ONLY when door is open
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!doorOpen) return;

        if (collision.CompareTag("Player"))
        {
            LevelManager.Instance.LoadNextRoom();
        }
    }

    // Player is touching a CLOSED door
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player")) return;

        PlayerController player = collision.collider.GetComponent<PlayerController>();
        player.SetDoorInteraction(true, doorOpen, transform.position.x);
    }

    // SUPER IMPORTANT — this keeps updating the touch every physics frame
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player")) return;

        PlayerController player = collision.collider.GetComponent<PlayerController>();
        player.SetDoorInteraction(true, doorOpen, transform.position.x);
    }

    // Player left the door → reset state
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player")) return;

        PlayerController player = collision.collider.GetComponent<PlayerController>();
        player.SetDoorInteraction(false, doorOpen, transform.position.x);
    }

    // Called when button is pressed
    public void OpenDoor()
    {
        doorOpen = true;
        col.isTrigger = true; // allow player to pass through
    }
}

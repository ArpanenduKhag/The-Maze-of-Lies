using UnityEngine;

public class Door : MonoBehaviour
{
    public bool doorOpen = false;
    private BoxCollider2D col;

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        doorOpen = false;      // Always reset when level prefab is loaded
    }

    private void Start()
    {
        col.isTrigger = false; // Door is solid at start
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (doorOpen && collision.CompareTag("Player"))
        {
            LevelManager.Instance.LoadNextRoom();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            var player = collision.collider.GetComponent<PlayerController>();
            player.SetDoorInteraction(true, doorOpen, transform.position.x);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            var player = collision.collider.GetComponent<PlayerController>();
            player.SetDoorInteraction(true, doorOpen, transform.position.x);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            var player = collision.collider.GetComponent<PlayerController>();
            player.SetDoorInteraction(false, doorOpen, transform.position.x);
        }
    }

    public void OpenDoor()
    {
        doorOpen = true;
        col.isTrigger = true; // Player can pass through
    }
}

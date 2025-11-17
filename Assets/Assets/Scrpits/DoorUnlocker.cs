using UnityEngine;

public class DoorUnlocker : MonoBehaviour
{
    public static DoorUnlocker Instance;

    private Door door;

    private void Awake()
    {
        Instance = this;
        door = GetComponent<Door>();
    }

    public void OpenDoor()
    {
        door.OpenDoor();
    }
}

using UnityEngine;

public class DoorUnlocker_Level3 : MonoBehaviour
{
    public Door door;

    public void OpenDoor()
    {
        door.OpenDoor();
        Debug.Log("Door Opened!");
    }
}

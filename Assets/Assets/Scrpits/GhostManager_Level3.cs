using UnityEngine;

public class GhostManager_Level3 : MonoBehaviour
{
    public static GhostManager_Level3 Instance;

    private int ghostsKilled = 0;
    private int totalGhosts = 0;

    private bool buttonPressed = false;
    public DoorUnlocker_Level3 door;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Count ghosts already in the level
        GhostCombatController[] ghosts =
            Object.FindObjectsByType<GhostCombatController>(FindObjectsSortMode.None);

        totalGhosts = ghosts.Length;

        Debug.Log("Initial Ghost Count = " + totalGhosts);
    }

    // Called when player dies and spawns a ghost
    public void GhostSpawned()
    {
        totalGhosts++;
        Debug.Log("Ghost Spawned -> Total now: " + totalGhosts);
    }

    // Called when a ghost is killed
    public void GhostKilled()
    {
        ghostsKilled++;
        Debug.Log("Ghost Killed (" + ghostsKilled + "/" + totalGhosts + ")");

        CheckDoorCondition();
    }

    public void ButtonPressed()
    {
        buttonPressed = true;
        CheckDoorCondition();
    }

    private void CheckDoorCondition()
    {
        if (buttonPressed && ghostsKilled >= totalGhosts)
        {
            Debug.Log("All ghosts dead + Button pressed -> OPEN DOOR");
            door.OpenDoor();
        }
    }
}

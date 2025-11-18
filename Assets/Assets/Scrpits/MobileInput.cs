using UnityEngine;

public class MobileInput : MonoBehaviour
{
    public static MobileInput Instance;

    [HideInInspector] public float horizontal;
    [HideInInspector] public bool jumpPressed;
    [HideInInspector] public bool slashPressed;

    private void Awake()
    {
        Instance = this;
    }

    public void LeftDown()  => horizontal = -1;
    public void LeftUp()    { if (horizontal < 0) horizontal = 0; }

    public void RightDown() => horizontal = 1;
    public void RightUp()   { if (horizontal > 0) horizontal = 0; }

    public void Jump()      => jumpPressed = true;
    public void Slash()     => slashPressed = true;

    private void LateUpdate()
    {
        jumpPressed = false;
        slashPressed = false;
    }
}

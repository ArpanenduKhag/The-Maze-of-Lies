using UnityEngine;

public class EnableMultiTouch : MonoBehaviour
{
    void Awake()
    {
        Input.multiTouchEnabled = true;
    }
}

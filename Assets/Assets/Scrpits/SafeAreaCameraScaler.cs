using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SafeAreaCameraScaler : MonoBehaviour
{
    private Camera cam;
    private float targetAspect = 16f / 9f;

    void Start()
    {
        cam = GetComponent<Camera>();
        ApplyAspect();
    }

    void ApplyAspect()
    {
        float screenAspect = (float)Screen.width / Screen.height;
        float scale = screenAspect / targetAspect;

        if (scale < 1f)
        {
            Rect rect = cam.rect;
            rect.width = 1f;
            rect.height = scale;
            rect.x = 0f;
            rect.y = (1f - scale) / 2f;
            cam.rect = rect;
        }
        else
        {
            float scaleWidth = 1f / scale;

            Rect rect = cam.rect;
            rect.width = scaleWidth;
            rect.height = 1f;
            rect.x = (1f - scaleWidth) / 2f;
            rect.y = 0f;
            cam.rect = rect;
        }
    }
}

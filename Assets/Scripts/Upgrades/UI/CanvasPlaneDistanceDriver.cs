using UnityEngine;

[RequireComponent(typeof(Canvas)), ExecuteAlways]
public class CanvasPlaneDistanceDriver : MonoBehaviour
{
    Canvas cached_Canvas;
    Canvas Canvas => cached_Canvas == null ? cached_Canvas = GetComponent<Canvas>() : cached_Canvas;

    public void Update()
    {
        if (!Canvas.worldCamera) return;
        if (Canvas.worldCamera.orthographic)
        {
            Canvas.planeDistance = 1;
            return;
        }
        var currentScale = Canvas.transform.lossyScale.x;
        Canvas.planeDistance /= currentScale;
    }
}

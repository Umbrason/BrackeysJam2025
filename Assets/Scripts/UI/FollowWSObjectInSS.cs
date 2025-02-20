using UnityEngine;

public class FollowWSObjectInSS : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Camera WSCamera;
    Cached<Canvas> cached_canvas = new(Cached<Canvas>.GetOption.Parent);
    Canvas Canvas => cached_canvas[this];
    Cached<RectTransform> cached_canvasRectTransform;
    RectTransform CanvasRectTransform => cached_canvasRectTransform[Canvas];
    void LateUpdate()
    {
        var viewport = WSCamera.WorldToViewportPoint(target.position);
        transform.localPosition = CanvasRectTransform.rect.size * ((Vector2)viewport - Vector2.one * .5f);
    }
}
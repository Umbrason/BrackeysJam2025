using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeDirectionMarker : MonoBehaviour
{
    [SerializeField] private Camera WSCamera;

    Cached<Canvas> cached_canvas = new(Cached<Canvas>.GetOption.Parent);
    Canvas Canvas => cached_canvas[this];

    Cached<RectTransform> cached_canvasRectTransform;
    RectTransform CanvasRectTransform => cached_canvasRectTransform[Canvas];
    void LateUpdate()
    {
        transform.GetChild(0).gameObject.SetActive(UpgradePickup.Instance != null);
        if (UpgradePickup.Instance == null)
            return;
        var viewport = WSCamera.WorldToViewportPoint(UpgradePickup.Instance.transform.position + Vector3.up * 2.5f);
        viewport *= Mathf.Sign(viewport.z);
        viewport = (Vector2)viewport - Vector2.one * .5f;
        var clampX = Mathf.Abs(.45f / viewport.x);
        var clampY = Mathf.Abs(.45f / viewport.y);
        var minClamp = Mathf.Min(1, Mathf.Min(clampX, clampY));
        transform.localPosition = CanvasRectTransform.rect.size * viewport * minClamp;
        var angle = minClamp < 1 ? Mathf.Atan2(viewport.y, viewport.x) * 180 / Mathf.PI : -90;
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }
}

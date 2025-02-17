using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private float smoothTime;
    [SerializeField] private float lookaheadRadius;
    private Vector3 currentVelocity = Vector3.zero;

    void Update()
    {
        var targetPosition = playerMovement.PlayerBody.velocity / playerMovement.Velocity * lookaheadRadius + playerMovement.PlayerBody.position;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);
    }
}

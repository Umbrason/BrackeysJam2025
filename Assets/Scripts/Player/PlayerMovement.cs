using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] bool canRotate = true;
    [field: SerializeField] public float Velocity { get; private set; }
    [SerializeField] private float deadzoneVelocityRadius;
    [SerializeField] private float maxVelocityRadius;
    [SerializeField] private Transform cameraAnchor;
    public Rigidbody PlayerBody { get; private set; }
    void Start()
    {
        PlayerBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        //Toggle Rotation
        if (!canRotate)
        {
            transform.rotation = Quaternion.identity;
        }

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var t = ray.origin.y / ray.direction.y;
        var xzIntersection = ray.origin + ray.direction * -t;
        var direction = xzIntersection - cameraAnchor.position;
        var mag = direction.magnitude;
        direction = direction.normalized;
        mag = Mathf.Clamp(mag - deadzoneVelocityRadius, 0, maxVelocityRadius - deadzoneVelocityRadius);
        mag /= maxVelocityRadius - deadzoneVelocityRadius;
        //if (!Input.GetMouseButton(0)) mag = 0; <<< require holding M1
        PlayerBody.velocity = mag * Velocity * direction;
    }

}

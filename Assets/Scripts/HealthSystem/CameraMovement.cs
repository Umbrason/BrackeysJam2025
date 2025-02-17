using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Vector3 offset;
    [SerializeField] private Transform target;
    [SerializeField] private float smoothTime;
    private Vector3 currentVelocity= Vector3.zero;

    private Quaternion freezeRotation;
    // Start is called before the first frame update
    void Start()
    {

        
        freezeRotation = transform.rotation;
        offset = transform.position - target.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = freezeRotation;
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);
        
    }
}

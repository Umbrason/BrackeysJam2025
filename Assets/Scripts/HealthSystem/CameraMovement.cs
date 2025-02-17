using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private float smoothTime;
    [SerializeField] private float lookaheadRadius;
    private Vector3 currentVelocity = Vector3.zero;

    [SerializeField] private Spring.Config springConfig;
    DrivenVector2Spring positionSpring;

    void Start()
    {
        positionSpring = new(() => (playerMovement.PlayerBody.velocity / playerMovement.Velocity * lookaheadRadius + playerMovement.PlayerBody.position)._xz(), springConfig);
        positionSpring.OnSpringUpdated += (pos) => transform.position = pos._x0y();
    }

    void Update()
    {
        positionSpring.Step(Time.deltaTime);


    }
}

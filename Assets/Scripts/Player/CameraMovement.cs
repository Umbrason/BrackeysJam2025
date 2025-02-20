using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private const string LOOK_AHEAD_RADIUS_KEY = "LookAheadRadius";

    [SerializeField] private Transform followTarget;
    [SerializeField] private float lookaheadRadius;
    public Vector2 LookAheadDirection { private get; set; }

    [SerializeField] private Spring.Config springConfig;
    DrivenVector2Spring positionSpring;

    void Start()
    {
        if(PersistantData.Instance.IsDataExists(LOOK_AHEAD_RADIUS_KEY))
        {
            PersistantData.Instance.TryGetData(LOOK_AHEAD_RADIUS_KEY, out lookaheadRadius);
        }
        positionSpring = new(() => LookAheadDirection * lookaheadRadius + followTarget.position._xz(), springConfig);
        positionSpring.OnSpringUpdated += (pos) => transform.position = pos._x0y();
    }

    void Update()
    {
        positionSpring.Step(Time.deltaTime);
    }
}

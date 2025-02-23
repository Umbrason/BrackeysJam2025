using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelocationCenter : MonoBehaviour
{
    public static Vector2 Position;
    void FixedUpdate()
    {
        Position = transform.position._xz();
    }
}

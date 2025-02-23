using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseWhileEnabled : MonoBehaviour
{
    Guid guid;
    void OnEnable() => guid = Pause.Request();
    void OnDisable() => Pause.Return(guid);
}

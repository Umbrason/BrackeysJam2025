using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetScoreOnLoad : MonoBehaviour
{
    void Awake() => TransientScoring.Restart();
}

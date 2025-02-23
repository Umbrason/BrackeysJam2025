using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistToLeaderboardOnUnload : MonoBehaviour
{
    async void OnDisable() => await TransientScoring.Save();
}

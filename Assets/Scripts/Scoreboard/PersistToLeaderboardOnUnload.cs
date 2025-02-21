using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistToLeaderboardOnUnload : MonoBehaviour
{
    void OnDisable()
    {
        //TODO: replace with call to leaderboard API
        Debug.Log(TransientScoring.TotalScore);
    }
}

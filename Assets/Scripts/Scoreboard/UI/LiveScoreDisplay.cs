using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LiveScoreDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text scoreText;
    void FixedUpdate()
    {
        timeText.text =  $"{Mathf.FloorToInt(TransientScoring.TimeAlive / 60f).ToString().PadLeft(2, '0')}:{(TransientScoring.TimeAlive % 60).ToString().PadLeft(2, '0')}";
        scoreText.text = $"{TransientScoring.TotalScore - TransientScoring.TimeAlive * TransientScoring.TimeAliveFactor}";
    }
}

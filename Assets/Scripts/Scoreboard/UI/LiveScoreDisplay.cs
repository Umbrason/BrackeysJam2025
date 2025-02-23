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
        timeText.text =  $"<mspace=1em>{Mathf.FloorToInt(TransientScoring.TimeAlive / 60f).ToString().PadLeft(2, '0')}</mspace><mspace=.5em>:</mspace><mspace=1em>{(TransientScoring.TimeAlive % 60).ToString().PadLeft(2, '0')}</mspace>";
        scoreText.text = $"<mspace=1em>{TransientScoring.TotalScore - TransientScoring.TimeAlive * TransientScoring.TimeAliveFactor}</mspace>";
    }
}

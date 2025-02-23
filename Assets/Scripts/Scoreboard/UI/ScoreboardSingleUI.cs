using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI scoreText;

    public void SetInfo(string playerName, int score)
    {   
        if (playerName != "")
        {
            playerNameText.transform.gameObject.SetActive(true);
            playerNameText.text = playerName;
        }
        else
        {
            playerNameText.transform.gameObject.SetActive(false);
        }
        
        scoreText.text = score.ToString();
    }
}

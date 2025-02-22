using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardUI : MonoBehaviour
{
    [SerializeField] private Button playerScoresButton;
    [SerializeField] private Button topScoresButton;
    [Min(4)]
    [SerializeField] private int maxEntriesCount = 5;
    [SerializeField] private ScoreboardSingleUI scoreboardSingleUITemplate;
    [SerializeField] private RectTransform content;

    private List<ScoreboardSingleUI> singleUIs;
    private async void ShowPlayerScores()
    {
        while(singleUIs.Count > 0)
        {
            int lastIndex = singleUIs.Count - 1;
            var singleUI = singleUIs[lastIndex];
            singleUIs.RemoveAt(lastIndex);
            Destroy(singleUI.gameObject);
        }

        var entries = await LeaderboardManager.GetScoresAroundPlayer(maxEntriesCount);

        float positionY = 0.0f;
        foreach (var entry in entries)
        {
            var singleUI = Instantiate(scoreboardSingleUITemplate, Vector3.zero, Quaternion.identity);
            singleUI.transform.SetParent(content.transform, false);
            
            RectTransform rectTransform = singleUI.transform.GetComponent<RectTransform>();
            Vector2 size = rectTransform.rect.size;
            rectTransform.pivot = new Vector2(0.5f, 1.0f);
            rectTransform.anchorMin = new Vector2(0.5f, 1.0f);
            rectTransform.anchorMax = new Vector2(0.5f, 1.0f);
            rectTransform.localPosition = new Vector3(size.x / 2.0f, positionY, 0.0f);
            positionY -= rectTransform.rect.size.y;

            singleUI.SetInfo("", (int)entry.Score);
            singleUIs.Add(singleUI);
        }
    }

    private async void ShowTopScores()
    {
        while (singleUIs.Count > 0)
        {
            int lastIndex = singleUIs.Count - 1;
            var singleUI = singleUIs[lastIndex];
            singleUIs.RemoveAt(lastIndex);
            Destroy(singleUI.gameObject);
        }

        var entries = await LeaderboardManager.GetBestScores(maxEntriesCount, 0);

        float positionY = 0.0f;

        foreach (var entry in entries)
        {
            var singleUI = Instantiate(scoreboardSingleUITemplate, Vector3.zero, Quaternion.identity);
            singleUI.transform.SetParent(content.transform, false);

            RectTransform rectTransform = singleUI.transform.GetComponent<RectTransform>();
            Vector2 size = rectTransform.rect.size;
            rectTransform.pivot = new Vector2(0.5f, 1.0f);
            rectTransform.anchorMin = new Vector2(0.5f, 1.0f);
            rectTransform.anchorMax = new Vector2(0.5f, 1.0f);
            rectTransform.localPosition = new Vector3(size.x / 2.0f, positionY, 0.0f);
            positionY -= rectTransform.rect.size.y;

            singleUI.SetInfo(entry.PlayerName, (int)entry.Score);
            singleUIs.Add(singleUI);
        }
    }

    private void Awake()
    {
        singleUIs = new List<ScoreboardSingleUI>();
    }

    private void OnEnable()
    {
        ShowPlayerScores();
    }

    private void Start()
    {
        playerScoresButton.onClick.AddListener(() => ShowPlayerScores());
        topScoresButton.onClick.AddListener(() => ShowTopScores());
    }

    private void OnDestroy()
    {
        playerScoresButton.onClick.RemoveAllListeners();
        topScoresButton.onClick.RemoveAllListeners();
    }
}

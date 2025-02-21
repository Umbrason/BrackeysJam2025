using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Exceptions;

using LeaderboardEntry = Unity.Services.Leaderboards.Models.LeaderboardEntry;
public static class LeaderboardManager
{
    private const string LEADERBOARD_ID = "Times-0";
    public static async void SetCurrentPlayerScore(int score)
    {
        int previousScore = 0;
        try
        {
            var previousEntry = await LeaderboardsService.Instance.GetPlayerScoreAsync(LEADERBOARD_ID);
            previousScore = (int)previousEntry.Score;
        }
        catch(LeaderboardsException e)
        {
            Debug.LogError("Exception while retreiving previous score: \n" + e);
            previousScore = -1;
        }

        if (previousScore > score)
        {
            Debug.Log("Already higher score, no need to update");
            return;
        }


        try
        {
            var playerEntry = await LeaderboardsService.Instance
            .AddPlayerScoreAsync(LEADERBOARD_ID, score);
        }
        catch (LeaderboardsException e)
        {
            Debug.LogError("Exception while setting new score: \n" + e);
        }
    }

    public static async Task<List<LeaderboardEntry>> GetScoresAroundPlayer(int rangeLimit)
    {
        try
        {
            var scores = await LeaderboardsService.Instance.GetPlayerRangeAsync(
                                LEADERBOARD_ID,
                                new GetPlayerRangeOptions { RangeLimit = rangeLimit }
                                );
            return scores.Results;
        }
        catch(LeaderboardsException e)
        {
            Debug.LogError("Error while getting scores around player: \n" + e);
            return new List<LeaderboardEntry>();
        }       
    }

    public static async Task<List<LeaderboardEntry>> GetBestScores(int rangeLimit, int offset)
    {
        try
        {
            var scores = await LeaderboardsService.Instance.GetScoresAsync(
                                LEADERBOARD_ID,
                                new GetScoresOptions { Limit = rangeLimit, Offset = offset }
                                );
            return scores.Results;
        }
        catch(LeaderboardsException e)
        {
            Debug.LogError("Error while getting best scores: \n" + e);
            return new List<LeaderboardEntry>();
        }
    }
}

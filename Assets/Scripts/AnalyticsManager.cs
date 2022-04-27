using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using GameAnalyticsSDK;

public class AnalyticsManager : MonoBehaviour
{
    public static  AnalyticsManager analyticsManager;
    private static int pausePressed=0;
    private static int startPressed=0;
    private static int playPressed=0;
   

    void Start()
    {
        analyticsManager = this;
    }

    public void GameIsPaused()
    {
        pausePressed++;
        GameAnalytics.NewDesignEvent($"Payer paused game {pausePressed} times");
    }
    public void GameIsPlayed()
    {
        playPressed++;
        GameAnalytics.NewDesignEvent($"Payer pressed play button {playPressed} times");
    }
    public void GameStarted()
    {
        startPressed++;
        GameAnalytics.NewDesignEvent($"Payer started game {startPressed} times");
    }
    public void MergeEvent(int numOfMerges)
    {
        GameAnalytics.NewDesignEvent($"Number of merges {numOfMerges} times");
    }
   

}

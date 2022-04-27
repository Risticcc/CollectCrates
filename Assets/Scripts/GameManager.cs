using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GameAnalyticsSDK;

public class GameManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject pauseMenu;
    public GameObject playPanel;
    public Button playButton; 
    public Button resumeButton; 
    public Button quitButton; 
    public Button pauseButton;

    public Text merges;
    public Text taps;

    public static GameManager Instance { get; private set; }
    private SpawnManager spawnMannager;


    

    void Start()
    {
        GameAnalyticsSDK.GameAnalytics.Initialize();

        mainMenu.gameObject.SetActive(true);
        pauseMenu.gameObject.SetActive(false);
        spawnMannager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    private void Update()
    {
        merges.text = "total merges: " + spawnMannager.NumberOfMerges;
        taps.text = "total taps: " + spawnMannager.SpawnedPrefabs;
    }

    public void Awake()
    {
        Instance = this;
    }

    public void StartGame()
    {

        mainMenu.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(false);
        playPanel.gameObject.SetActive(true);
        AnalyticsManager.analyticsManager.GameIsPlayed();
        Debug.Log(AnalyticsManager.analyticsManager);
    }
    
    public void ResumeGame()
    {
        mainMenu.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(false);
        playPanel.gameObject.SetActive(true);
        Time.timeScale = 1;
        AnalyticsManager.analyticsManager.GameIsPlayed();
    }

    public void PauseGame()
    {
        mainMenu.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(true);
        playPanel.gameObject.SetActive(false);
        AnalyticsManager.analyticsManager.GameIsPaused();
        Time.timeScale = 0;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

 

}

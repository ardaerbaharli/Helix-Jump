using System;
using Ads;
using Enums;
using UnityEngine;
using Utilities;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private PlayerController playerController;


    public Action OnGameOver;
    public Action OnPlayerContinuesPlaying;
    public Action OnRewardedAdCompleted;

    private Helix previousHelix, prePreviousHelix;
    public static GameManager instance;
    public GameState State { get; set; }

    public bool IsInitialized => ObjectPool.instance.isPoolSet;

    private void Awake()
    {
        Application.targetFrameRate = 30;
        Config.DeathCount = 0;
        Time.timeScale = 1;
        
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartGame()
    {
        State = GameState.Playing;
    }

    private void Start()
    {
        State = GameState.Menu;
        ScoreManager.instance.OnScored += OnScored;
        AdManager.instance.OnRewardedAdCompleted += OnRewardedAdCompletedCallback;
    }


    private void OnScored(int score)
    {
        if (score > ScoreManager.instance.HighScore)
            ScoreManager.instance.HighScore = score;

        Vibration.Light();

        if (prePreviousHelix != null)
            prePreviousHelix.SelfDestroy();

        playerController.activeHelix.DeactivateColliders();
        prePreviousHelix = previousHelix;
        previousHelix = playerController.activeHelix;
        playerController.activeHelix = levelManager.ActiveHelix;

        levelManager.LoadLevel();
    }


    public void GameOver()
    {
        State = GameState.GameOver;
        Config.DeathCount++;
        OnGameOver?.Invoke();
    }


    public void Resume()
    {
        Time.timeScale = 1;
        State = GameState.Playing;
    }

    public void Pause()
    {
        Time.timeScale = 0;
        State = GameState.Paused;
    }

    public void KeepPlaying()
    {
        AdManager.instance.ShowRewardedAd();
    }

    private void OnRewardedAdCompletedCallback()
    {
        OnRewardedAdCompleted?.Invoke();
    }

    public void CameraMoveFinished()
    {
        State = GameState.Playing;
        ScoreManager.instance.Score++;
        OnPlayerContinuesPlaying?.Invoke();
    }

    public void EndGame()
    {
        State = GameState.GameOver;
        if (Config.DeathCount % 5 == 0) AdManager.instance.ShowInterstitialAd();
        PageController.Instance.ShowPage(Pages.GameOver);
    }
}
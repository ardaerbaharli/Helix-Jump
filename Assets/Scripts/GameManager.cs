using System;
using Ads;
using Enums;
using UnityEngine;
using Utilities;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Ball ball;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private CameraController cameraController;

    private int deathCount;
    public Action OnGameOver;

    public Action OnPlayerContinuesPlaying;
    private Helix previousHelix, prePreviousHelix, nextHelix;
    public static GameManager instance;
    public GameState State { get; set; }

    public bool IsInitialized => ObjectPool.instance.isPoolSet;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        deathCount = 0;

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
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
    }


    private void OnScored(int score)
    {
        if (score > ScoreManager.instance.HighScore)
            ScoreManager.instance.HighScore = score;

        Vibration.Light();

        if (superSpeedCollisionHappened)
        {
            superSpeedCollisionHappened = false;
            cameraController.isPassiveSpeedOn = true;
        }

        if (score - ball.lastTouchedLevel >= 3)
        {
            ball.ToggleSuperSpeed(true);
        }

        playerController.activeHelix.BlowUp();

        if (prePreviousHelix != null)
            prePreviousHelix.SelfDestroy();

        prePreviousHelix = previousHelix;
        previousHelix = playerController.activeHelix;
        playerController.activeHelix = levelManager.ActiveHelix;

        levelManager.LoadLevel();
    }

    private bool superSpeedCollisionHappened = false;

    public void SuperSpeedCollision()
    {
        SoundManager.instance.PlayHelixBreak();
        playerController.activeHelix.BlowUp();
        superSpeedCollisionHappened = true;
        cameraController.isPassiveSpeedOn = false;
    }

    public void GameOver()
    {
        print("game over");
        State = GameState.GameOver;
        deathCount++;
        OnGameOver?.Invoke();
    }


    public void Resume()
    {
        State = GameState.Playing;
        Time.timeScale = 1;
    }

    public void Pause()
    {
        State = GameState.Paused;
        Time.timeScale = 0;
    }

    public void KeepPlaying()
    {
        AdManager.instance.ShowRewardedAd();
        AdManager.instance.OnRewardedAdCompleted += OnRewardedAdCompleted;
    }

    private void OnRewardedAdCompleted()
    {
        OnPlayerContinuesPlaying?.Invoke();
    }

    public void CameraMoveFinished()
    {
        State = GameState.Playing;
    }

    public void EndGame()
    {
        print(deathCount % 5 == 0);
        if (deathCount % 5 == 0) AdManager.instance.ShowInterstitialAd();
        PageController.Instance.ShowPage(Pages.GameOver);
    }
}
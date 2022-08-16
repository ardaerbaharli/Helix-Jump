using System;
using Enums;
using UnityEngine;
using Utilities;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Ball ball;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private PlayerController playerController;

    public Action OnGameOver;
    private Helix previousHelix;
    public static GameManager instance;
    public GameState State { get; set; }

    public bool IsInitialized => ObjectPool.instance.isPoolSet;

    private void Awake()
    {
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
    }


    private void OnScored(int score)
    {
        if (score > ScoreManager.instance.HighScore)
            ScoreManager.instance.HighScore = score;
        Vibration.Light();

        playerController.didSuperSpeedCollideHappened = false;

        if (score - ball.lastTouchedLevel >= 3)
        {
            ball.ToggleSuperSpeed(true);
        }

        playerController.activeHelix.BlowUp();

        if (previousHelix != null)
            previousHelix.SelfDestroy();

        previousHelix = playerController.activeHelix;
        playerController.activeHelix = levelManager.ActiveHelix;

        levelManager.LoadLevel();
    }


    public void SuperSpeedCollision()
    {
        playerController.SuperSpeedCollision();
    }

    public void GameOver()
    {
        print("game over");
        State = GameState.GameOver;
        OnGameOver?.Invoke();
        PageController.Instance.ShowPage(Pages.GameOver);
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
}
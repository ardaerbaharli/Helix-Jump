using TMPro;
using UnityEngine;
using Utilities;

public class ScoreManager : MonoBehaviour
{
    public delegate void OnScoredDelegate(int score);

    public event OnScoredDelegate OnScored;
    private int score;

    public static ScoreManager instance;

    public int Score
    {
        get => score;
        set
        {
            score = value;
            OnScored?.Invoke(score);
        }
    }

    public int HighScore
    {
        get => PlayerPrefs.GetInt(Config.HighScorePref, 0);
        set => PlayerPrefs.SetInt(Config.HighScorePref, value);
    }

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
}
using System;
using Enums;
using TMPro;
using UnityEngine;

namespace UI
{
    public class GamePanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private GameObject adPrompt;

        private void Start()
        {
            ScoreManager.instance.OnScored += OnScored;
            GameManager.instance.OnGameOver += OnGameOver;
        }

        private void OnDestroy()
        {
            ScoreManager.instance.OnScored -= OnScored;
            GameManager.instance.OnGameOver -= OnGameOver;
        }

        private void OnGameOver()
        {
            adPrompt.SetActive(true);
        }

        public void ShowAd()
        {
            adPrompt.SetActive(false);
            GameManager.instance.KeepPlaying();
        }

        public void EndGame()
        {
            adPrompt.SetActive(false);
            GameManager.instance.EndGame();
        }

        private void OnScored(int score)
        {
            scoreText.text = score.ToString();
        }

        public void Pause()
        {
            SoundManager.instance.PlayUiClick();
            GameManager.instance.Pause();
            PageController.Instance.ShowPage(Pages.Pause);
        }
    }
}
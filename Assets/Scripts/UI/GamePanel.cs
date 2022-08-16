using System;
using Enums;
using TMPro;
using UnityEngine;

namespace UI
{
    public class GamePanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;

        private void Start()
        {
            ScoreManager.instance.OnScored += OnScored;
        }

        private void OnScored(int score)
        {
            scoreText.text = score.ToString();
        }

        public void Pause()
        {
            GameManager.instance.Pause();
            PageController.Instance.ShowPage(Pages.Pause);
        }
        
    }
}
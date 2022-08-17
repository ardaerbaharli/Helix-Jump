using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class GameOverPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI highScoreText;

        private void OnEnable()
        {
            scoreText.text = $"score: {ScoreManager.instance.Score}";
            highScoreText.text = $"high score: {ScoreManager.instance.HighScore}";
        }

        public void Restart()
        {
            SoundManager.instance.PlayUiClick();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void Settings()
        {
            SoundManager.instance.PlayUiClick();
            PageController.Instance.ShowPage(Pages.Settings);
        }
    }
}
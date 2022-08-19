using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private Image fillBackground;
        [SerializeField] private TextMeshProUGUI currentText;
        [SerializeField] private TextMeshProUGUI targetText;
        [SerializeField] private float fillSpeed;
        [SerializeField] private float colorChangeSpeed;

        [SerializeField] private Color startColor;
        [SerializeField] private Color halfwayColor;
        [SerializeField] private Color fullColor;

        private int targetScore, halfwayScore;

        private void Start()
        {
            currentText.text = "0";
            targetScore = ScoreManager.instance.HighScore;
            if (targetScore == 0)
                targetScore = 1;

            halfwayScore = Mathf.FloorToInt(targetScore / 2f);

            targetText.text = targetScore.ToString();

            fillBackground.fillAmount = 0;
            fillBackground.color = startColor;

            ScoreManager.instance.OnScored += OnScored;
        }

        private void OnScored(int score)
        {
            UpdateProgress(score);
        }

        private void UpdateProgress(int score)
        {
            currentText.text = score.ToString();

            if (score == halfwayScore)
                StartCoroutine(SetColor(halfwayColor));
            else if (score == targetScore)
                StartCoroutine(SetColor(fullColor));

            StartCoroutine(UpdateFillAmount(score));
        }

        private IEnumerator SetColor(Color targetColor)
        {
            while (!fillBackground.color.Compare(targetColor))
            {
                fillBackground.color = Color.Lerp(fillBackground.color, targetColor, colorChangeSpeed * Time.deltaTime);
                yield return null;
            }

        }

        private IEnumerator UpdateFillAmount(int score)
        {
            var targetFillAmount = (float) score / ScoreManager.instance.HighScore;
            while (fillBackground.fillAmount < targetFillAmount)
            {
                fillBackground.fillAmount = Mathf.MoveTowards(fillBackground.fillAmount, targetFillAmount,
                    fillSpeed * Time.deltaTime);
                yield return null;
            }
        }
    }
}
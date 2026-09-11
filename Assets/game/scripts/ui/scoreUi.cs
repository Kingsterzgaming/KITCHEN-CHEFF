using TMPro;
using UnityEngine;
using YesChef.Core;

namespace YesChef.UI
{
    public class ScoreUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private HighScoreManager highScoreManager;

        [Header("UI")]
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text highScoreText;

        private void Update()
        {
            UpdateScore();
            UpdateHighScore();
        }

        private void UpdateScore()
        {
            if (scoreText == null || scoreManager == null)
                return;

            scoreText.text =
                $"Score: {scoreManager.CurrentScore}";
        }

        private void UpdateHighScore()
        {
            if (highScoreText == null ||
                highScoreManager == null)
            {
                return;
            }

            highScoreText.text =
                $"High Score: {highScoreManager.HighScore}";
        }
    }
}
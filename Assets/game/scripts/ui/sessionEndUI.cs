using TMPro;
using UnityEngine;
using YesChef.Core;

namespace YesChef.UI
{
    public class SessionEndUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameSession gameSession;
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private HighScoreManager highScoreManager;

        [Header("UI")]
        [SerializeField] private GameObject endPanel;
        [SerializeField] private TMP_Text finalScoreText;
        [SerializeField] private TMP_Text highScoreText;
        [SerializeField] private TMP_Text newHighScoreText;

        private void OnEnable()
        {
            if (gameSession != null)
            {
                gameSession.SessionFinished +=
                    HandleSessionFinished;
            }
        }

        private void OnDisable()
        {
            if (gameSession != null)
            {
                gameSession.SessionFinished -=
                    HandleSessionFinished;
            }
        }

        private void Start()
        {
            HidePanel();
        }

        private void HandleSessionFinished()
        {
            ShowResults();
        }

        private void ShowResults()
        {
            if (endPanel == null)
                return;

            int finalScore =
                scoreManager != null
                    ? scoreManager.CurrentScore
                    : 0;

            int highScore =
                highScoreManager != null
                    ? highScoreManager.HighScore
                    : 0;

            if (finalScoreText != null)
            {
                finalScoreText.text =
                    $"Score: {finalScore}";
            }

            if (highScoreText != null)
            {
                highScoreText.text =
                    $"High Score: {highScore}";
            }

            if (newHighScoreText != null)
            {
                bool isNewHighScore =
                    finalScore > 0 &&
                    finalScore >= highScore;

                newHighScoreText.gameObject.SetActive(
                    isNewHighScore
                );
            }

            endPanel.SetActive(true);
        }

        public void RestartGame()
        {
            HidePanel();

            if (gameSession == null)
                return;

            gameSession.ResetSession();
            gameSession.StartSession();
        }

        public void HidePanel()
        {
            if (endPanel != null)
            {
                endPanel.SetActive(false);
            }
        }
    }
}
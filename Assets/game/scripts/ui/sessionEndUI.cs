using TMPro;
using UnityEngine;
using YesChef.Core;

namespace YesChef.UI
{
    public class SessionEndUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameManager gameManager;
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
                gameSession.SessionFinished += HandleSessionFinished;
        }

        private void OnDisable()
        {
            if (gameSession != null)
                gameSession.SessionFinished -= HandleSessionFinished;
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

            int previousHighScore =
                highScoreManager != null
                    ? highScoreManager.HighScore
                    : 0;

            bool isNewHighScore =
                finalScore > previousHighScore;

            if (gameManager != null)
            {
                gameManager.HighScoreManager.TrySetHighScore(
                    finalScore
                );
            }

            int currentHighScore =
                highScoreManager != null
                    ? highScoreManager.HighScore
                    : finalScore;

            if (finalScoreText != null)
            {
                finalScoreText.text =
                    $"Score: {finalScore}";
            }

            if (highScoreText != null)
            {
                highScoreText.text =
                    $"High Score: {currentHighScore}";
            }

            if (newHighScoreText != null)
            {
                newHighScoreText.gameObject.SetActive(
                    isNewHighScore
                );
            }

            endPanel.SetActive(true);
        }

        public void RestartGame()
        {
            HidePanel();

            if (gameManager == null)
            {
                Debug.LogError(
                    "SessionEndUI: GameManager is not assigned.",
                    this
                );

                return;
            }

            gameManager.ResetGame();
            gameManager.StartGame();
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public void HidePanel()
        {
            if (endPanel != null)
                endPanel.SetActive(false);
        }
    }
}
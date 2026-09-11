using UnityEngine;

namespace YesChef.Core
{
    public class GameManager : MonoBehaviour
    {
        [Header("Systems")]
        [SerializeField] private GameSession gameSession;
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private HighScoreManager highScoreManager;

        public GameSession GameSession => gameSession;
        public ScoreManager ScoreManager => scoreManager;
        public HighScoreManager HighScoreManager => highScoreManager;

        private void OnEnable()
        {
            if (gameSession != null)
            {
                gameSession.SessionFinished += HandleSessionFinished;
            }
        }

        private void OnDisable()
        {
            if (gameSession != null)
            {
                gameSession.SessionFinished -= HandleSessionFinished;
            }
        }

        private void Awake()
        {
            ValidateReferences();
        }

        private void ValidateReferences()
        {
            if (gameSession == null)
            {
                Debug.LogError(
                    "GameManager: GameSession is not assigned.",
                    this
                );
            }

            if (scoreManager == null)
            {
                Debug.LogError(
                    "GameManager: ScoreManager is not assigned.",
                    this
                );
            }

            if (highScoreManager == null)
            {
                Debug.LogError(
                    "GameManager: HighScoreManager is not assigned.",
                    this
                );
            }
        }

        public void StartGame()
        {
            if (gameSession == null)
                return;

            if (scoreManager != null)
            {
                scoreManager.ResetScore();
            }

            gameSession.StartSession();
        }

        public void PauseGame()
        {
            if (gameSession == null)
                return;

            gameSession.PauseSession();
        }

        public void ResumeGame()
        {
            if (gameSession == null)
                return;

            gameSession.ResumeSession();
        }

        public void ResetGame()
        {
            if (gameSession == null)
                return;

            gameSession.ResetSession();

            if (scoreManager != null)
            {
                scoreManager.ResetScore();
            }
        }

        private void HandleSessionFinished()
        {
            if (scoreManager == null ||
                highScoreManager == null)
            {
                return;
            }

            highScoreManager.TrySetHighScore(
                scoreManager.CurrentScore
            );
        }
    }
}
using System;
using UnityEngine;

namespace YesChef.Core
{
    public class GameSession : MonoBehaviour
    {
        public enum SessionState
        {
            NotStarted,
            Playing,
            Paused,
            Finished
        }

        [Header("References")]
        [SerializeField] private GameTimer gameTimer;

        private SessionState currentState =
            SessionState.NotStarted;

        public SessionState CurrentState => currentState;

        public bool IsPlaying =>
            currentState == SessionState.Playing;

        public bool IsPaused =>
            currentState == SessionState.Paused;

        public bool IsFinished =>
            currentState == SessionState.Finished;

        public event Action SessionStarted;
        public event Action SessionPaused;
        public event Action SessionResumed;
        public event Action SessionFinished;

        private void OnEnable()
        {
            if (gameTimer != null)
            {
                gameTimer.TimerFinished += HandleTimerFinished;
            }
        }

        private void OnDisable()
        {
            if (gameTimer != null)
            {
                gameTimer.TimerFinished -= HandleTimerFinished;
            }
        }

        public void StartSession()
        {
            if (currentState != SessionState.NotStarted)
                return;

            if (gameTimer == null)
            {
                Debug.LogError(
                    "GameSession: GameTimer is not assigned.",
                    this
                );

                return;
            }

            currentState = SessionState.Playing;

            gameTimer.ResetTimer();
            gameTimer.StartTimer();

            SessionStarted?.Invoke();
        }

        public void PauseSession()
        {
            if (currentState != SessionState.Playing)
                return;

            currentState = SessionState.Paused;

            gameTimer.PauseTimer();

            Time.timeScale = 0f;

            SessionPaused?.Invoke();
        }

        public void ResumeSession()
        {
            if (currentState != SessionState.Paused)
                return;

            Time.timeScale = 1f;

            currentState = SessionState.Playing;

            gameTimer.StartTimer();

            SessionResumed?.Invoke();
        }

        public void EndSession()
        {
            if (currentState == SessionState.Finished)
                return;

            Time.timeScale = 1f;

            currentState = SessionState.Finished;

            gameTimer.PauseTimer();

            SessionFinished?.Invoke();
        }

        public void ResetSession()
        {
            Time.timeScale = 1f;

            if (gameTimer != null)
            {
                gameTimer.ResetTimer();
            }

            currentState = SessionState.NotStarted;
        }

        private void HandleTimerFinished()
        {
            EndSession();
        }


    }
}
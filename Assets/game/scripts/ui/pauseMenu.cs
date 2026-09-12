using UnityEngine;
using UnityEngine.InputSystem;
using YesChef.Core;

namespace YesChef.UI
{
    public class PauseMenuUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameSession gameSession;
        [SerializeField] private InputActionReference pauseAction;

        [Header("UI")]
        [SerializeField] private GameObject pausePanel;

        private void OnEnable()
        {
            if (gameSession != null)
            {
                gameSession.SessionStarted += HandleSessionStarted;
                gameSession.SessionFinished += HandleSessionFinished;
            }

            if (pauseAction != null)
                pauseAction.action.Enable();
        }

        private void OnDisable()
        {
            if (gameSession != null)
            {
                gameSession.SessionStarted -= HandleSessionStarted;
                gameSession.SessionFinished -= HandleSessionFinished;
            }

            if (pauseAction != null)
                pauseAction.action.Disable();
        }

        private void Start()
        {
            HidePauseMenu();
        }

        private void Update()
        {
            if (pauseAction == null)
                return;

            if (!pauseAction.action.WasPressedThisFrame())
                return;

            TogglePause();
        }

        public void TogglePause()
        {
            if (gameSession == null)
                return;

            if (gameSession.IsFinished)
                return;

            if (gameSession.IsPlaying)
                PauseGame();
            else if (gameSession.IsPaused)
                ResumeGame();
        }

        public void PauseGame()
        {
            if (gameSession == null)
                return;

            gameSession.PauseSession();
            ShowPauseMenu();
        }

        public void ResumeGame()
        {
            if (gameSession == null)
                return;

            gameSession.ResumeSession();
            HidePauseMenu();
        }

        public void ResetGame()
        {
            if (gameSession == null)
                return;

            gameSession.ResetSession();
            HidePauseMenu();
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void HandleSessionStarted()
        {
            HidePauseMenu();
        }

        private void HandleSessionFinished()
        {
            HidePauseMenu();
        }

        private void ShowPauseMenu()
        {
            if (pausePanel != null)
                pausePanel.SetActive(true);
        }

        private void HidePauseMenu()
        {
            if (pausePanel != null)
                pausePanel.SetActive(false);
        }
    }
}
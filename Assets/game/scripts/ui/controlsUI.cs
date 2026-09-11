using UnityEngine;
using YesChef.Core;

namespace YesChef.UI
{
    public class ControlsUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameSession gameSession;

        [Header("UI")]
        [SerializeField] private GameObject controlsPanel;

        private void OnEnable()
        {
            if (gameSession != null)
            {
                gameSession.SessionStarted += HandleSessionStarted;
            }
        }

        private void OnDisable()
        {
            if (gameSession != null)
            {
                gameSession.SessionStarted -= HandleSessionStarted;
            }
        }

        private void Start()
        {
            ShowControls();
        }

        private void HandleSessionStarted()
        {
            HideControls();
        }

        public void ShowControls()
        {
            if (controlsPanel != null)
            {
                controlsPanel.SetActive(true);
            }
        }

        public void HideControls()
        {
            if (controlsPanel != null)
            {
                controlsPanel.SetActive(false);
            }
        }
    }
}
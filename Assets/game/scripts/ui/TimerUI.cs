using TMPro;
using UnityEngine;
using YesChef.Core;

namespace YesChef.UI
{
    public class TimerUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameTimer gameTimer;

        [Header("UI")]
        [SerializeField] private TMP_Text timerText;

        private void Update()
        {
            if (gameTimer == null ||
                timerText == null)
            {
                return;
            }

            UpdateTimerText();
        }

        private void UpdateTimerText()
        {
            int totalSeconds =
                Mathf.CeilToInt(
                    gameTimer.RemainingTime
                );

            int minutes =
                totalSeconds / 60;

            int seconds =
                totalSeconds % 60;

            timerText.text =
                $"{minutes:00}:{seconds:00}";
        }
    }
}
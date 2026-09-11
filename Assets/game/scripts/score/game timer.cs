using System;
using UnityEngine;

namespace YesChef.Core
{
    public class GameTimer : MonoBehaviour
    {
        [Header("Session")]
        [SerializeField] private float sessionDuration = 180f;

        private float remainingTime;
        private bool isRunning;

        public float RemainingTime => remainingTime;
        public float ElapsedTime =>
            sessionDuration - remainingTime;

        public bool IsRunning => isRunning;
        public bool IsFinished => remainingTime <= 0f;

        public event Action TimerStarted;
        public event Action TimerFinished;

        private void Update()
        {
            if (!isRunning)
                return;

            remainingTime -= Time.deltaTime;

            if (remainingTime <= 0f)
            {
                remainingTime = 0f;
                isRunning = false;

                TimerFinished?.Invoke();
            }
        }

        public void StartTimer()
        {
            if (IsFinished)
                return;

            isRunning = true;
            TimerStarted?.Invoke();
        }

        public void PauseTimer()
        {
            isRunning = false;
        }

        public void ResetTimer()
        {
            remainingTime = sessionDuration;
            isRunning = false;
        }

        public void SetDuration(float duration)
        {
            if (duration <= 0f)
                return;

            sessionDuration = duration;
            remainingTime = duration;
            isRunning = false;
        }
    }
}
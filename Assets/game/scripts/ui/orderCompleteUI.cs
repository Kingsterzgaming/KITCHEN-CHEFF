using System.Collections;
using TMPro;
using UnityEngine;
using YesChef.Core;
using YesChef.Orders;

namespace YesChef.UI
{
    public class OrderCompletionUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CustomerWindow customerWall;
        [SerializeField] private ScoreManager scoreManager;

        [Header("Popup")]
        [SerializeField] private GameObject popup;
        [SerializeField] private TMP_Text scoreText;

        [Header("Settings")]
        [SerializeField] private float displayDuration = 2f;

        private Coroutine hideCoroutine;

        private void OnEnable()
        {
            if (customerWall != null)
            {
                customerWall.OrderCompleted +=
                    HandleOrderCompleted;
            }
        }

        private void OnDisable()
        {
            if (customerWall != null)
            {
                customerWall.OrderCompleted -=
                    HandleOrderCompleted;
            }
        }

        private void Start()
        {
            HidePopup();
        }

        private void HandleOrderCompleted(
            CustomerWindow wall,
            int windowIndex,
            Order completedOrder)
        {
            if (scoreManager == null ||
                completedOrder == null)
            {
                return;
            }

            int score =
                scoreManager.CalculateOrderScore(
                    completedOrder
                );

            ShowPopup(
                windowIndex,
                score
            );
        }

        private void ShowPopup(
            int windowIndex,
            int score)
        {
            if (popup == null)
                return;

            if (scoreText != null)
            {
                scoreText.text =
                    score >= 0
                        ? $"+{score}"
                        : score.ToString();
            }

            Transform windowPoint =
                customerWall.GetWindowPoint(
                    windowIndex
                );

            if (windowPoint != null)
            {
                transform.position =
                    windowPoint.position;
            }

            popup.SetActive(true);

            if (hideCoroutine != null)
            {
                StopCoroutine(hideCoroutine);
            }

            hideCoroutine =
                StartCoroutine(
                    HideAfterDelay()
                );
        }

        private IEnumerator HideAfterDelay()
        {
            yield return new WaitForSeconds(
                displayDuration
            );

            HidePopup();
        }

        private void HidePopup()
        {
            if (popup != null)
            {
                popup.SetActive(false);
            }
        }
    }
}
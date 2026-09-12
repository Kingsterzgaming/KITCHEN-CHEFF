using UnityEngine;
using YesChef.Core;

namespace YesChef.UI
{
    public class OrdersUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameSession gameSession;
        [SerializeField] private GameObject ordersPanel;

        private void OnEnable()
        {
            if (gameSession != null)
            {
                gameSession.SessionStarted += HandleSessionStarted;
                gameSession.SessionFinished += HandleSessionFinished;
            }
        }

        private void OnDisable()
        {
            if (gameSession != null)
            {
                gameSession.SessionStarted -= HandleSessionStarted;
                gameSession.SessionFinished -= HandleSessionFinished;
            }
        }

        private void Start()
        {
            HideOrders();
        }

        private void HandleSessionStarted()
        {
            ShowOrders();
        }

        private void HandleSessionFinished()
        {
            HideOrders();
        }

        private void ShowOrders()
        {
            if (ordersPanel != null)
                ordersPanel.SetActive(true);
        }

        private void HideOrders()
        {
            if (ordersPanel != null)
                ordersPanel.SetActive(false);
        }
    }
}
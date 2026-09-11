using System.Collections;
using UnityEngine;
using YesChef.Core;

namespace YesChef.Orders
{
    public class OrderManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CustomerWindow customerWall;
        [SerializeField] private GameSession gameSession;

        [Header("Orders")]
        [SerializeField] private float respawnDelay = 5f;

        private OrderGenerator orderGenerator;

        private void Awake()
        {
            orderGenerator = new OrderGenerator();
        }

        private void OnEnable()
        {
            if (gameSession != null)
            {
                gameSession.SessionStarted += HandleSessionStarted;
                gameSession.SessionFinished += HandleSessionFinished;
            }

            if (customerWall != null)
            {
                customerWall.OrderCompleted += HandleOrderCompleted;
            }
        }

        private void OnDisable()
        {
            if (gameSession != null)
            {
                gameSession.SessionStarted -= HandleSessionStarted;
                gameSession.SessionFinished -= HandleSessionFinished;
            }

            if (customerWall != null)
            {
                customerWall.OrderCompleted -= HandleOrderCompleted;
            }
        }

        private void HandleSessionStarted()
        {
            StopAllCoroutines();

            ClearAllOrders();
            GenerateInitialOrders();
        }

        private void HandleSessionFinished()
        {
            StopAllCoroutines();

            ClearAllOrders();
        }

        private void HandleOrderCompleted(
            CustomerWindow wall,
            int windowIndex,
            Order completedOrder)
        {
            if (gameSession == null ||
                !gameSession.IsPlaying)
            {
                return;
            }

            StartCoroutine(
                RespawnOrder(windowIndex)
            );
        }

        private IEnumerator RespawnOrder(
            int windowIndex)
        {
            customerWall.ClearOrder(windowIndex);

            yield return new WaitForSeconds(
                respawnDelay
            );

            if (gameSession == null ||
                !gameSession.IsPlaying)
            {
                yield break;
            }

            Order newOrder =
                orderGenerator.GenerateOrder();

            customerWall.SetOrder(
                windowIndex,
                newOrder
            );
        }

        private void GenerateInitialOrders()
        {
            if (customerWall == null)
            {
                Debug.LogError(
                    "OrderManager: Customer Wall is not assigned.",
                    this
                );

                return;
            }

            for (int i = 0;
                 i < customerWall.WindowCount;
                 i++)
            {
                Order order =
                    orderGenerator.GenerateOrder();

                customerWall.SetOrder(i, order);
            }
        }

        private void ClearAllOrders()
        {
            if (customerWall == null)
                return;

            for (int i = 0;
                 i < customerWall.WindowCount;
                 i++)
            {
                customerWall.ClearOrder(i);
            }
        }
    }
}
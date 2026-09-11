using System.Collections;
using UnityEngine;

namespace YesChef.Orders
{
    public class OrderManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CustomerWindow customerWall;

        [Header("Orders")]
        [SerializeField] private float respawnDelay = 5f;

        private OrderGenerator orderGenerator;

        private void Awake()
        {
            orderGenerator = new OrderGenerator();
        }

        private void OnEnable()
        {
            if (customerWall != null)
            {
                customerWall.OrderCompleted += HandleOrderCompleted;
            }
        }

        private void OnDisable()
        {
            if (customerWall != null)
            {
                customerWall.OrderCompleted -= HandleOrderCompleted;
            }
        }

        private void Start()
        {
            GenerateInitialOrders();
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

            for (int i = 0; i < customerWall.WindowCount; i++)
            {
                Order order = orderGenerator.GenerateOrder();

                customerWall.SetOrder(i, order);
            }
        }

        private void HandleOrderCompleted(
            CustomerWindow wall,
            int windowIndex,
            Order completedOrder)
        {
            StartCoroutine(
                RespawnOrder(windowIndex)
            );
        }

        private IEnumerator RespawnOrder(int windowIndex)
        {
            customerWall.ClearOrder(windowIndex);

            yield return new WaitForSeconds(respawnDelay);

            Order newOrder =
                orderGenerator.GenerateOrder();

            customerWall.SetOrder(
                windowIndex,
                newOrder
            );
        }
    }
}
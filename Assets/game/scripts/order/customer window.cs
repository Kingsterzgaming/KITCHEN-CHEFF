using System;
using UnityEngine;
using YesChef.Ingredients;
using YesChef.Player;

namespace YesChef.Orders
{
    public class CustomerWindow : MonoBehaviour
    {
        [Serializable]
        private class WindowPoint
        {
            [SerializeField] private Transform point;

            private Order currentOrder;

            public Transform Point => point;
            public Order CurrentOrder => currentOrder;
            public bool HasOrder => currentOrder != null;

            public void SetOrder(Order order)
            {
                currentOrder = order;
            }

            public void ClearOrder()
            {
                currentOrder = null;
            }
        }

        [Header("Customer Windows")]
        [SerializeField]
        private WindowPoint[] windows = new WindowPoint[4];

        public int WindowCount =>
            windows != null ? windows.Length : 0;

        public event Action<CustomerWindow, int, Order>
            OrderCompleted;

        // --------------------------------------------------
        // ORDER MANAGEMENT
        // --------------------------------------------------

        public void SetOrder(int windowIndex, Order order)
        {
            if (!IsValidIndex(windowIndex))
                return;

            if (order == null)
            {
                Debug.LogWarning(
                    $"Cannot assign a null order to window {windowIndex}.",
                    this
                );

                return;
            }

            windows[windowIndex].SetOrder(order);
        }

        public void ClearOrder(int windowIndex)
        {
            if (!IsValidIndex(windowIndex))
                return;

            windows[windowIndex].ClearOrder();
        }

        public Order GetOrder(int windowIndex)
        {
            if (!IsValidIndex(windowIndex))
                return null;

            return windows[windowIndex].CurrentOrder;
        }

        public Transform GetWindowPoint(int windowIndex)
        {
            if (!IsValidIndex(windowIndex))
                return null;

            return windows[windowIndex].Point;
        }

        // --------------------------------------------------
        // ORDER PROGRESS
        // --------------------------------------------------

        public int GetCompletedCount(int windowIndex)
        {
            Order order = GetOrder(windowIndex);

            if (order == null)
                return 0;

            return order.DeliveredIngredients.Count;
        }

        public int GetRemainingCount(int windowIndex)
        {
            Order order = GetOrder(windowIndex);

            if (order == null)
                return 0;

            return order.RequiredIngredients.Count -
                   order.DeliveredIngredients.Count;
        }

        // --------------------------------------------------
        // WINDOW-SPECIFIC INTERACTION
        // --------------------------------------------------

        public bool CanInteract(
            PlayerController player,
            int windowIndex)
        {
            if (player == null)
                return false;

            if (!IsValidIndex(windowIndex))
                return false;

            if (!player.Inventory.HasIngredient)
                return false;

            Order order =
                windows[windowIndex].CurrentOrder;

            if (order == null)
                return false;

            IngredientInstance ingredient =
                player.Inventory.HeldIngredient;

            return order.CanAccept(ingredient);
        }

        public void Interact(
            PlayerController player,
            int windowIndex)
        {
            if (!CanInteract(player, windowIndex))
                return;

            Order order =
                windows[windowIndex].CurrentOrder;

            IngredientInstance ingredient =
                player.Inventory.HeldIngredient;

            if (!order.TryDeliver(ingredient))
                return;

            player.Inventory.RemoveIngredient();

            Destroy(ingredient.gameObject);

            if (!order.IsComplete)
                return;

            OrderCompleted?.Invoke(
                this,
                windowIndex,
                order
            );
        }

        // --------------------------------------------------
        // VALIDATION
        // --------------------------------------------------

        private bool IsValidIndex(int index)
        {
            return windows != null &&
                   index >= 0 &&
                   index < windows.Length;
        }
    }
}
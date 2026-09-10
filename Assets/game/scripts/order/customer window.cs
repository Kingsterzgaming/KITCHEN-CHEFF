using System;
using UnityEngine;
using YesChef.Ingredients;
using YesChef.Player;
using YesChef.Stations;

namespace YesChef.Orders
{
    public class CustomerWindow : MonoBehaviour, IInteractable
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
        private WindowPoint[] windows =
            new WindowPoint[4];

        public int WindowCount => windows.Length;

        public event Action<CustomerWindow, int, Order> OrderCompleted;

        public void SetOrder(int windowIndex, Order order)
        {
            if (!IsValidIndex(windowIndex))
                return;

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

        public bool CanInteract(PlayerController player)
        {
            if (player == null)
                return false;

            if (!player.Inventory.HasIngredient)
                return false;

            int windowIndex = GetClosestWindow(player);

            if (windowIndex == -1)
                return false;

            Order order = windows[windowIndex].CurrentOrder;

            if (order == null)
                return false;

            return order.CanAccept(
                player.Inventory.HeldIngredient
            );
        }

        public void Interact(PlayerController player)
        {
            if (!CanInteract(player))
                return;

            int windowIndex = GetClosestWindow(player);

            if (windowIndex == -1)
                return;

            WindowPoint window = windows[windowIndex];

            Order order = window.CurrentOrder;

            if (order == null)
                return;

            IngredientInstance ingredient =
                player.Inventory.HeldIngredient;

            if (!order.TryDeliver(ingredient))
                return;

            player.Inventory.RemoveIngredient();

            Destroy(ingredient.gameObject);

            if (order.IsComplete)
            {
                OrderCompleted?.Invoke(
                    this,
                    windowIndex,
                    order
                );
            }
        }

        private int GetClosestWindow(PlayerController player)
        {
            if (windows == null || windows.Length == 0)
                return -1;

            int closestIndex = -1;
            float closestDistance = float.MaxValue;

            for (int i = 0; i < windows.Length; i++)
            {
                WindowPoint window = windows[i];

                if (window == null || window.Point == null)
                    continue;

                float distance =
                    Vector3.Distance(
                        player.transform.position,
                        window.Point.position
                    );

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestIndex = i;
                }
            }

            return closestIndex;
        }

        private bool IsValidIndex(int index)
        {
            return windows != null &&
                   index >= 0 &&
                   index < windows.Length;
        }
    }
}
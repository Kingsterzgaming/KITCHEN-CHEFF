using TMPro;
using UnityEngine;
using YesChef.Ingredients;
using YesChef.Orders;

namespace YesChef.UI
{
    public class OrderUI : MonoBehaviour
    {
        [Header("Order")]
        [SerializeField] private CustomerWindow customerWall;
        [SerializeField] private int windowIndex;

        [Header("Text")]
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private TMP_Text progressText;

        [Header("Ingredient Slots")]
        [SerializeField] private IngredientIconUI[] ingredientIcons;

        [Header("Ingredient Sprites")]
        [SerializeField] private Sprite vegetableSprite;
        [SerializeField] private Sprite cheeseSprite;
        [SerializeField] private Sprite meatSprite;

        private void Update()
        {
            Order order = GetCurrentOrder();

            if (order == null)
            {
                ClearUI();
                return;
            }

            UpdateTimer(order);
            UpdateProgress(order);
            UpdateIngredients(order);
        }

        private Order GetCurrentOrder()
        {
            if (customerWall == null)
                return null;

            return customerWall.GetOrder(windowIndex);
        }

        private void UpdateTimer(Order order)
        {
            if (timerText == null)
                return;

            timerText.text =
                $"{order.ElapsedSeconds:00}s";
        }

        private void UpdateProgress(Order order)
        {
            if (progressText == null)
                return;

            int completed =
                order.DeliveredIngredients.Count;

            int total =
                order.RequiredIngredients.Count;

            int remaining =
                total - completed;

            progressText.text =
                $"{completed}/{total}  •  {remaining} left";
        }

        private void UpdateIngredients(Order order)
        {
            if (ingredientIcons == null)
                return;

            for (int i = 0;
                 i < ingredientIcons.Length;
                 i++)
            {
                IngredientIconUI icon =
                    ingredientIcons[i];

                if (icon == null)
                    continue;

                if (i >= order.RequiredIngredients.Count)
                {
                    icon.Clear();
                    continue;
                }

                IngredientType requiredType =
                    order.RequiredIngredients[i];

                icon.SetIcon(
                    GetSprite(requiredType)
                );

                icon.SetDelivered(
                    IsIngredientDelivered(
                        order,
                        i
                    )
                );
            }
        }

        private bool IsIngredientDelivered(
            Order order,
            int requiredIndex)
        {
            IngredientType requiredType =
                order.RequiredIngredients[requiredIndex];

            int requiredCount = 0;
            int deliveredCount = 0;

            for (int i = 0;
                 i <= requiredIndex;
                 i++)
            {
                if (order.RequiredIngredients[i] ==
                    requiredType)
                {
                    requiredCount++;
                }
            }

            foreach (IngredientType deliveredType
                     in order.DeliveredIngredients)
            {
                if (deliveredType == requiredType)
                {
                    deliveredCount++;
                }
            }

            return deliveredCount >= requiredCount;
        }

        private Sprite GetSprite(
            IngredientType type)
        {
            switch (type)
            {
                case IngredientType.Vegetable:
                    return vegetableSprite;

                case IngredientType.Cheese:
                    return cheeseSprite;

                case IngredientType.Meat:
                    return meatSprite;

                default:
                    return null;
            }
        }

        private void ClearUI()
        {
            if (timerText != null)
            {
                timerText.text = "--";
            }

            if (progressText != null)
            {
                progressText.text = "--/--";
            }

            if (ingredientIcons == null)
                return;

            foreach (IngredientIconUI icon
                     in ingredientIcons)
            {
                if (icon != null)
                    icon.Clear();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;
using YesChef.Ingredients;

namespace YesChef.Orders
{
    public class Order
    {
        private readonly List<IngredientType> requiredIngredients;
        private readonly List<IngredientType> deliveredIngredients;

        private readonly float creationTime;

        public IReadOnlyList<IngredientType> RequiredIngredients =>
            requiredIngredients;

        public IReadOnlyList<IngredientType> DeliveredIngredients =>
            deliveredIngredients;

        public bool IsComplete =>
            deliveredIngredients.Count == requiredIngredients.Count;

        public int IngredientCount =>
            requiredIngredients.Count;

        public float OpenDuration =>
            Time.time - creationTime;

        public Order(List<IngredientType> ingredients)
        {
            if (ingredients == null || ingredients.Count == 0)
            {
                throw new ArgumentException(
                    "An order must contain at least one ingredient."
                );
            }

            requiredIngredients =
                new List<IngredientType>(ingredients);

            deliveredIngredients =
                new List<IngredientType>();

            creationTime = Time.time;
        }

        public bool CanAccept(IngredientInstance ingredient)
        {
            if (ingredient == null)
            {
                return false;
            }

            if (!ingredient.IsPrepared &&
                ingredient.Type != IngredientType.Cheese)
            {
                return false;
            }

            return requiredIngredients.Contains(ingredient.Type);
        }

        public bool TryDeliver(IngredientInstance ingredient)
        {
            if (!CanAccept(ingredient))
            {
                return false;
            }

            IngredientType type = ingredient.Type;

            int requiredCount =
                CountIngredient(requiredIngredients, type);

            int deliveredCount =
                CountIngredient(deliveredIngredients, type);

            if (deliveredCount >= requiredCount)
            {
                return false;
            }

            deliveredIngredients.Add(type);

            return true;
        }

        public int CalculateScore(
            IReadOnlyDictionary<IngredientType, int> scoreValues)
        {
            if (scoreValues == null)
            {
                throw new ArgumentNullException(nameof(scoreValues));
            }

            int ingredientScore = 0;

            foreach (IngredientType ingredient in requiredIngredients)
            {
                if (!scoreValues.TryGetValue(
                        ingredient,
                        out int value))
                {
                    Debug.LogWarning(
                        $"No score value found for {ingredient}."
                    );

                    continue;
                }

                ingredientScore += value;
            }

            int elapsedSeconds =
                Mathf.FloorToInt(OpenDuration);

            return ingredientScore - elapsedSeconds;
        }

        private int CountIngredient(
            List<IngredientType> ingredients,
            IngredientType type)
        {
            int count = 0;

            foreach (IngredientType ingredient in ingredients)
            {
                if (ingredient == type)
                {
                    count++;
                }
            }

            return count;
        }
    }
}
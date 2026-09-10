using System.Collections.Generic;
using UnityEngine;
using YesChef.Ingredients;

namespace YesChef.Orders
{
    public class OrderGenerator
    {
        private readonly IngredientType[] availableIngredients =
        {
            IngredientType.Vegetable,
            IngredientType.Cheese,
            IngredientType.Meat
        };

        public Order GenerateOrder()
        {
            int ingredientCount = Random.value < 0.5f ? 2 : 3;

            List<IngredientType> ingredients =
                new List<IngredientType>(ingredientCount);

            for (int i = 0; i < ingredientCount; i++)
            {
                IngredientType ingredient =
                    GetRandomIngredient();

                ingredients.Add(ingredient);
            }

            return new Order(ingredients);
        }

        private IngredientType GetRandomIngredient()
        {
            int index =
                Random.Range(0, availableIngredients.Length);

            return availableIngredients[index];
        }
    }
}
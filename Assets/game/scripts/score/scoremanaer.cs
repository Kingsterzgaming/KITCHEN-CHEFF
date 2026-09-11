using System.Collections.Generic;
using UnityEngine;
using YesChef.Ingredients;
using YesChef.Orders;

namespace YesChef.Core
{
    public class ScoreManager : MonoBehaviour
    {
        private readonly Dictionary<IngredientType, int> scoreValues =
            new Dictionary<IngredientType, int>();

        private int currentScore;

        public int CurrentScore => currentScore;

        public void Initialize(
            IngredientDefinition[] ingredientDefinitions)
        {
            scoreValues.Clear();

            if (ingredientDefinitions == null)
                return;

            foreach (IngredientDefinition definition in ingredientDefinitions)
            {
                if (definition == null)
                    continue;

                scoreValues[definition.Type] =
                    definition.ScoreValue;
            }
        }

        public int CalculateOrderScore(Order order)
        {
            if (order == null)
                return 0;

            return order.CalculateScore(scoreValues);
        }

        public int AddOrderScore(Order order)
        {
            int score = CalculateOrderScore(order);

            currentScore += score;

            return score;
        }

        public void ResetScore()
        {
            currentScore = 0;
        }
    }
}
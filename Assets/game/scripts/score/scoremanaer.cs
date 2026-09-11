using System.Collections.Generic;
using UnityEngine;
using YesChef.Ingredients;
using YesChef.Orders;

namespace YesChef.Core
{
    public class ScoreManager : MonoBehaviour
    {
        [Header("Ingredient Definitions")]
        [SerializeField] private IngredientDefinition[] ingredientDefinitions;

        [Header("Score")]
        [SerializeField] private int currentScore;
        [SerializeField] private CustomerWindow customerWall;
        private readonly Dictionary<IngredientType, int> scoreValues =
            new Dictionary<IngredientType, int>();

        public int CurrentScore => currentScore;

        private void Awake()
        {
            BuildScoreTable();
        }

        //private void OnEnable()
        //{
        //    FindAndSubscribeToOrderManager();
        //}

        //private void OnDisable()
        //{
        //    UnsubscribeFromOrderManager();
        //}

        private void OnEnable()
        {
            if (customerWall != null)
                customerWall.OrderCompleted += HandleOrderCompleted;
        }

        private void OnDisable()
        {
            if (customerWall != null)
                customerWall.OrderCompleted -= HandleOrderCompleted;
        }

        private void BuildScoreTable()
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

        //private void FindAndSubscribeToOrderManager()
        //{
        //    OrderManager orderManager =
        //        FindFirstObjectByType<OrderManager>();

        //    if (orderManager == null)
        //        return;

        //    CustomerWindow customerWall =
        //        FindFirstObjectByType<CustomerWindow>();

        //    if (customerWall == null)
        //        return;

        //    customerWall.OrderCompleted += HandleOrderCompleted;
        //}

        //private void UnsubscribeFromOrderManager()
        //{
        //    CustomerWindow customerWall =
        //        FindFirstObjectByType<CustomerWindow>();

        //    if (customerWall == null)
        //        return;

        //    customerWall.OrderCompleted -= HandleOrderCompleted;
        //}

        private void HandleOrderCompleted(
            CustomerWindow wall,
            int windowIndex,
            Order completedOrder)
        {
            AddOrderScore(completedOrder);
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
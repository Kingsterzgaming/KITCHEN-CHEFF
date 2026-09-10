using UnityEngine;

namespace YesChef.Ingredients
{
    public class IngredientInstance : MonoBehaviour
    {
        private IngredientDefinition definition;
        private IngredientState state;

        public IngredientDefinition Definition => definition;
        public IngredientType Type => definition.Type;
        public IngredientState State => state;

        public bool IsPrepared =>
            state == IngredientState.Prepared;

        public void Initialize(IngredientDefinition ingredientDefinition)
        {
            if (ingredientDefinition == null)
            {
                Debug.LogError(
                    "Cannot initialize IngredientInstance without a definition.",
                    this
                );

                return;
            }

            definition = ingredientDefinition;
            state = IngredientState.Raw;
        }

        public void Prepare()
        {
            if (definition == null)
            {
                return;
            }

            if (!definition.RequiresPreparation)
            {
                return;
            }

            state = IngredientState.Prepared;
        }
    }
}
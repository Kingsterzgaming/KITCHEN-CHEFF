using UnityEngine;

namespace YesChef.Ingredients
{
    public class IngredientInstance : MonoBehaviour
    {
        private IngredientDefinition definition;
        private IngredientState state;

        private IngredientVisual visual;

        public IngredientDefinition Definition => definition;

        public IngredientType Type => definition.Type;

        public IngredientState State => state;

        public bool IsPrepared =>
            state == IngredientState.Prepared;

        private void Awake()
        {
            visual = GetComponent<IngredientVisual>();
        }

        public void Initialize(IngredientDefinition ingredientDefinition)
        {
            if (ingredientDefinition == null)
            {
                Debug.LogError(
                    $"IngredientInstance on {gameObject.name} " +
                    "received a null definition.",
                    this
                );

                return;
            }

            definition = ingredientDefinition;
            state = IngredientState.Raw;

            UpdateVisual();
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

            if (IsPrepared)
            {
                return;
            }

            state = IngredientState.Prepared;

            UpdateVisual();
        }

        private void UpdateVisual()
        {
            if (visual != null)
            {
                visual.SetPrepared(IsPrepared);
            }
        }
    }
}
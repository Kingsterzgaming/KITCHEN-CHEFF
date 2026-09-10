using UnityEngine;

namespace YesChef.Ingredients
{
    [CreateAssetMenu(
        fileName = "IngredientDefinition",
        menuName = "Yes Chef/Ingredients/Ingredient Definition"
    )]
    public class IngredientDefinition : ScriptableObject
    {
        [Header("Ingredient")]
        [SerializeField]
        private IngredientType type;

        [Header("Scoring")]
        [SerializeField]
        private int scoreValue;

        [Header("Preparation")]
        [SerializeField]
        private bool requiresPreparation;

        [SerializeField]
        private float preparationTime;

        public IngredientType Type => type;
        public int ScoreValue => scoreValue;
        public bool RequiresPreparation => requiresPreparation;
        public float PreparationTime => preparationTime;
    }
}
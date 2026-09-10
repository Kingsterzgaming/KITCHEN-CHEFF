using UnityEngine;
using YesChef.Ingredients;
using YesChef.Player;

namespace YesChef.Stations
{
    public class Refrigerator : MonoBehaviour, IInteractable
    {
        [Header("Available Ingredients")]
        [SerializeField]
        private IngredientDefinition[] ingredients;

        [Header("Spawn")]
        [SerializeField]
        private Transform spawnPoint;

        public bool CanInteract(PlayerController player)
        {
            return player != null &&
                   player.Inventory.IsEmpty &&
                   ingredients != null &&
                   ingredients.Length > 0;
        }

        public void Interact(PlayerController player)
        {
            if (!CanInteract(player))
            {
                return;
            }

            IngredientDefinition definition =
                ingredients[Random.Range(0, ingredients.Length)];

            IngredientInstance ingredient =
                CreateIngredient(definition);

            if (!player.Inventory.TryPickup(ingredient))
            {
                Destroy(ingredient.gameObject);
            }
        }

        private IngredientInstance CreateIngredient(
     IngredientDefinition definition)
        {
            GameObject ingredientObject =
                new GameObject(definition.Type.ToString());

            IngredientInstance instance =
                ingredientObject.AddComponent<IngredientInstance>();

            instance.Initialize(definition);

            ingredientObject.transform.position =
                spawnPoint != null
                    ? spawnPoint.position
                    : transform.position;

            return instance;
        }
    }
}
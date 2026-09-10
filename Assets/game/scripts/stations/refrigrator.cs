using UnityEngine;
using YesChef.Ingredients;
using YesChef.Player;

namespace YesChef.Stations
{
    public class Refrigerator : MonoBehaviour, IInteractable
    {
        [System.Serializable]
        private class IngredientEntry
        {
            public IngredientDefinition definition;
            public IngredientInstance prefab;
        }

        [Header("Available Ingredients")]
        [SerializeField]
        private IngredientEntry[] ingredients;

        [Header("Spawn")]
        [SerializeField]
        private Transform spawnPoint;

        public bool CanInteract(PlayerController player)
        {
            if (player == null)
            {
                return false;
            }

            if (!player.Inventory.IsEmpty)
            {
                return false;
            }

            return ingredients != null &&
                   ingredients.Length > 0;
        }

        public void Interact(PlayerController player)
        {
            if (!CanInteract(player))
            {
                return;
            }

            IngredientEntry entry = GetRandomIngredient();

            if (entry == null || entry.prefab == null)
            {
                Debug.LogError(
                    "Refrigerator has an invalid ingredient entry.",
                    this
                );

                return;
            }

            IngredientInstance ingredient =
                InstantiateIngredient(entry);

            if (ingredient == null)
            {
                return;
            }

            if (!player.Inventory.TryPickup(ingredient))
            {
                Destroy(ingredient.gameObject);
            }
        }

        private IngredientEntry GetRandomIngredient()
        {
            return ingredients[
                Random.Range(0, ingredients.Length)
            ];
        }

        private IngredientInstance InstantiateIngredient(
            IngredientEntry entry)
        {
            Vector3 position = spawnPoint != null
                ? spawnPoint.position
                : transform.position;

            Quaternion rotation = spawnPoint != null
                ? spawnPoint.rotation
                : Quaternion.identity;

            IngredientInstance ingredient =
                Instantiate(
                    entry.prefab,
                    position,
                    rotation
                );

            ingredient.Initialize(entry.definition);

            return ingredient;
        }
    }
}
using UnityEngine;
using YesChef.Ingredients;
using YesChef.Player;

namespace YesChef.Stations
{
    public class ChoppingTable : MonoBehaviour, IInteractable
    {
        [Header("Ingredient Placement")]
        [SerializeField]
        private Transform ingredientPoint;

        private IngredientInstance currentIngredient;
        private float remainingTime;

        public bool IsBusy => currentIngredient != null;

        public float RemainingTime => remainingTime;

        public bool CanInteract(PlayerController player)
        {
            if (player == null)
            {
                return false;
            }

            // Take finished vegetable.
            if (IsBusy)
            {
                return remainingTime <= 0f &&
                       player.Inventory.IsEmpty;
            }

            // Place vegetable for chopping.
            if (player.Inventory.HasIngredient)
            {
                IngredientInstance ingredient =
                    player.Inventory.HeldIngredient;

                return ingredient.Type == IngredientType.Vegetable &&
                       !ingredient.IsPrepared;
            }

            return false;
        }

        public void Interact(PlayerController player)
        {
            if (!CanInteract(player))
            {
                return;
            }

            if (!IsBusy)
            {
                StartChopping(player);
            }
            else
            {
                CollectIngredient(player);
            }
        }

        private void StartChopping(PlayerController player)
        {
            currentIngredient =
                player.Inventory.RemoveIngredient();

            if (currentIngredient == null)
            {
                return;
            }

            remainingTime =
                currentIngredient.Definition.PreparationTime;

            currentIngredient.transform.SetParent(
                ingredientPoint != null
                    ? ingredientPoint
                    : transform
            );

            currentIngredient.transform.localPosition =
                Vector3.zero;

            currentIngredient.transform.localRotation =
                Quaternion.identity;
        }

        private void Update()
        {
            if (!IsBusy || remainingTime <= 0f)
            {
                return;
            }

            remainingTime -= Time.deltaTime;

            if (remainingTime <= 0f)
            {
                remainingTime = 0f;
                currentIngredient.Prepare();
            }
        }

        private void CollectIngredient(PlayerController player)
        {
            if (currentIngredient == null)
            {
                return;
            }

            IngredientInstance ingredient =
                currentIngredient;

            currentIngredient = null;

            ingredient.transform.SetParent(null);

            bool pickedUp =
                player.Inventory.TryPickup(ingredient);

            if (!pickedUp)
            {
                // Put it back if player's hand became unavailable.
                currentIngredient = ingredient;

                ingredient.transform.SetParent(
                    ingredientPoint != null
                        ? ingredientPoint
                        : transform
                );

                ingredient.transform.localPosition =
                    Vector3.zero;
            }
        }
    }
}
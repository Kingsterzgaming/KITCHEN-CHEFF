using UnityEngine;
using YesChef.Ingredients;
using YesChef.Player;

namespace YesChef.Stations
{
    public class ChoppingTable : MonoBehaviour, IInteractable
    {
        private IngredientInstance currentIngredient;
        private float remainingTime;

        public bool IsBusy =>
            currentIngredient != null;

        public float RemainingTime =>
            remainingTime;

        public bool CanInteract(PlayerController player)
        {
            if (player == null)
            {
                return false;
            }

            // Player can place an ingredient on the table.
            if (!IsBusy && player.Inventory.HasIngredient)
            {
                IngredientInstance ingredient =
                    player.Inventory.HeldIngredient;

                return ingredient.Type ==
                       IngredientType.Vegetable &&
                       !ingredient.IsPrepared;
            }

            // Player can collect finished ingredient.
            if (IsBusy &&
                remainingTime <= 0f &&
                player.Inventory.IsEmpty)
            {
                return true;
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

            currentIngredient.transform.SetParent(transform);
            currentIngredient.transform.localPosition =
                Vector3.up * 0.5f;
        }

        private void Update()
        {
            if (!IsBusy)
            {
                return;
            }

            if (remainingTime <= 0f)
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

            player.Inventory.TryPickup(ingredient);
        }
    }
}
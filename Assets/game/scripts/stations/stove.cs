using UnityEngine;
using YesChef.Ingredients;
using YesChef.Player;

namespace YesChef.Stations
{
    public class Stove : MonoBehaviour, IInteractable
    {
        [System.Serializable]
        private class CookingSlot
        {
            public Transform visualPoint;

            [HideInInspector]
            public IngredientInstance ingredient;

            [HideInInspector]
            public float remainingTime;

            public bool IsEmpty =>
                ingredient == null;

            public bool IsFinished =>
                ingredient != null &&
                remainingTime <= 0f;
        }

        [SerializeField]
        private CookingSlot[] slots = new CookingSlot[2];

        public bool CanInteract(PlayerController player)
        {
            if (player == null)
            {
                return false;
            }

            // Player can place raw meat.
            if (player.Inventory.HasIngredient)
            {
                IngredientInstance ingredient =
                    player.Inventory.HeldIngredient;

                if (ingredient.Type != IngredientType.Meat ||
                    ingredient.IsPrepared)
                {
                    return false;
                }

                return FindEmptySlot() != null;
            }

            // Player can collect cooked meat.
            if (player.Inventory.IsEmpty)
            {
                return FindFinishedSlot() != null;
            }

            return false;
        }

        public void Interact(PlayerController player)
        {
            if (!CanInteract(player))
            {
                return;
            }

            if (player.Inventory.HasIngredient)
            {
                PlaceMeat(player);
            }
            else
            {
                CollectMeat(player);
            }
        }

        private void PlaceMeat(PlayerController player)
        {
            CookingSlot slot = FindEmptySlot();

            if (slot == null)
            {
                return;
            }

            IngredientInstance ingredient =
                player.Inventory.RemoveIngredient();

            if (ingredient == null)
            {
                return;
            }

            slot.ingredient = ingredient;
            slot.remainingTime =
                ingredient.Definition.PreparationTime;

            ingredient.transform.SetParent(
                slot.visualPoint != null
                    ? slot.visualPoint
                    : transform
            );

            ingredient.transform.localPosition =
                Vector3.zero;
        }

        private void Update()
        {
            foreach (CookingSlot slot in slots)
            {
                if (slot == null ||
                    slot.ingredient == null ||
                    slot.remainingTime <= 0f)
                {
                    continue;
                }

                slot.remainingTime -= Time.deltaTime;

                if (slot.remainingTime <= 0f)
                {
                    slot.remainingTime = 0f;
                    slot.ingredient.Prepare();
                }
            }
        }

        private void CollectMeat(PlayerController player)
        {
            CookingSlot slot = FindFinishedSlot();

            if (slot == null)
            {
                return;
            }

            IngredientInstance ingredient =
                slot.ingredient;

            slot.ingredient = null;

            ingredient.transform.SetParent(null);

            player.Inventory.TryPickup(ingredient);
        }

        private CookingSlot FindEmptySlot()
        {
            foreach (CookingSlot slot in slots)
            {
                if (slot != null && slot.IsEmpty)
                {
                    return slot;
                }
            }

            return null;
        }

        private CookingSlot FindFinishedSlot()
        {
            foreach (CookingSlot slot in slots)
            {
                if (slot != null && slot.IsFinished)
                {
                    return slot;
                }
            }

            return null;
        }
    }
}
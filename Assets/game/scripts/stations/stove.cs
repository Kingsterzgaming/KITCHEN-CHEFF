using System;
using UnityEngine;
using YesChef.Ingredients;
using YesChef.Player;

namespace YesChef.Stations
{
    public class Stove : MonoBehaviour, IInteractable
    {
        [Serializable]
        private class CookingSlot
        {
            [SerializeField]
            private Transform ingredientPoint;

            private IngredientInstance ingredient;
            private float remainingTime;

            public Transform IngredientPoint => ingredientPoint;

            public IngredientInstance Ingredient => ingredient;

            public float RemainingTime => remainingTime;

            public bool IsEmpty => ingredient == null;

            public bool IsFinished =>
                ingredient != null && remainingTime <= 0f;

            public void Place(IngredientInstance newIngredient)
            {
                ingredient = newIngredient;

                remainingTime =
                    newIngredient.Definition.PreparationTime;
            }

            public IngredientInstance Remove()
            {
                IngredientInstance result = ingredient;

                ingredient = null;
                remainingTime = 0f;

                return result;
            }

            public void Process(float deltaTime)
            {
                if (ingredient == null || remainingTime <= 0f)
                {
                    return;
                }

                remainingTime -= deltaTime;

                if (remainingTime <= 0f)
                {
                    remainingTime = 0f;
                    ingredient.Prepare();
                }
            }
        }

        [Header("Cooking Slots")]
        [SerializeField]
        private CookingSlot[] slots = new CookingSlot[2];

        public bool CanInteract(PlayerController player)
        {
            if (player == null)
            {
                return false;
            }

            // Player is holding something.
            if (player.Inventory.HasIngredient)
            {
                IngredientInstance ingredient =
                    player.Inventory.HeldIngredient;

                // Only raw meat can be placed here.
                if (ingredient.Type != IngredientType.Meat ||
                    ingredient.IsPrepared)
                {
                    return false;
                }

                return HasEmptySlot();
            }

            // Player has empty hands.
            // Allow collecting cooked meat.
            return FindFinishedSlot() != null;
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

        private void Update()
        {
            if (Time.timeScale <= 0f)
                return;

            foreach (CookingSlot slot in slots)
            {
                if (slot != null)
                    slot.Process(Time.deltaTime);
            }
        }

        private void PlaceMeat(PlayerController player)
        {
            CookingSlot slot = FindEmptySlot();

            if (slot == null)
            {
                return;
            }

            IngredientInstance meat =
                player.Inventory.RemoveIngredient();

            if (meat == null)
            {
                return;
            }

            slot.Place(meat);

            Transform point = slot.IngredientPoint;

            meat.transform.SetParent(
                point != null ? point : transform
            );

            meat.transform.localPosition = Vector3.zero;
            meat.transform.localRotation = Quaternion.identity;
        }

        private void CollectMeat(PlayerController player)
        {
            CookingSlot slot = FindFinishedSlot();

            if (slot == null)
            {
                return;
            }

            IngredientInstance meat = slot.Remove();

            if (meat == null)
            {
                return;
            }

            meat.transform.SetParent(null);

            bool pickedUp =
                player.Inventory.TryPickup(meat);

            if (!pickedUp)
            {
                // If pickup fails, put the meat back into the slot.
                slot.Place(meat);

                meat.transform.SetParent(
                    slot.IngredientPoint != null
                        ? slot.IngredientPoint
                        : transform
                );

                meat.transform.localPosition = Vector3.zero;
                meat.transform.localRotation = Quaternion.identity;
            }
        }

        private bool HasEmptySlot()
        {
            return FindEmptySlot() != null;
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
using UnityEngine;
using YesChef.Ingredients;

namespace YesChef.Player
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField]
        private Transform handPoint;

        private IngredientInstance heldIngredient;

        public IngredientInstance HeldIngredient => heldIngredient;

        public bool HasIngredient => heldIngredient != null;
        public bool IsEmpty => heldIngredient == null;

        public bool TryPickup(IngredientInstance ingredient)
        {
            if (ingredient == null || HasIngredient)
            {
                return false;
            }

            heldIngredient = ingredient;

            ingredient.transform.SetParent(handPoint);
            ingredient.transform.localPosition = Vector3.zero;
            ingredient.transform.localRotation = Quaternion.identity;

            return true;
        }

        public IngredientInstance RemoveIngredient()
        {
            IngredientInstance ingredient = heldIngredient;

            if (ingredient == null)
            {
                return null;
            }

            heldIngredient = null;

            ingredient.transform.SetParent(null);

            return ingredient;
        }

        public IngredientInstance DropIngredient()
        {
            return RemoveIngredient();
        }
    }
}
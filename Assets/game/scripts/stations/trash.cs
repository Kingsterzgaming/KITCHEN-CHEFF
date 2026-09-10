using UnityEngine;
using YesChef.Player;

namespace YesChef.Stations
{
    public class Trash : MonoBehaviour, IInteractable
    {
        public bool CanInteract(PlayerController player)
        {
            return player != null &&
                   player.Inventory.HasIngredient;
        }

        public void Interact(PlayerController player)
        {
            if (!CanInteract(player))
            {
                return;
            }

            var ingredient =
                player.Inventory.RemoveIngredient();

            if (ingredient != null)
            {
                Destroy(ingredient.gameObject);
            }
        }
    }
}
using UnityEngine;
using YesChef.Player;
using YesChef.Stations;

namespace YesChef.Orders
{
    public class CustomerWindowPoint : MonoBehaviour, IInteractable
    {
        [Header("References")]
        [SerializeField] private CustomerWindow customerWall;

        [SerializeField] private int windowIndex;

        public int WindowIndex => windowIndex;

        public bool CanInteract(PlayerController player)
        {
            if (customerWall == null)
                return false;

            return customerWall.CanInteract(
                player,
                windowIndex
            );
        }

        public void Interact(PlayerController player)
        {
            if (customerWall == null)
                return;

            customerWall.Interact(
                player,
                windowIndex
            );
        }
    }
}
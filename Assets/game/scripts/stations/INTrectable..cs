using YesChef.Player;

namespace YesChef.Stations
{
    public interface IInteractable
    {
        void Interact(PlayerController player);

        bool CanInteract(PlayerController player);
    }
}
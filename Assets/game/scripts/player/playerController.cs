using UnityEngine;
using YesChef.Stations;

namespace YesChef.Player
{
    [RequireComponent(typeof(PlayerInventory))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField]
        private float moveSpeed = 5f;

        [Header("Interaction")]
        [SerializeField]
        private float interactionRange = 1.5f;

        [SerializeField]
        private LayerMask interactableLayer;

        private PlayerInventory inventory;
        private IInteractable currentInteractable;

        private void Awake()
        {
            inventory = GetComponent<PlayerInventory>();
        }

        private void Update()
        {
            HandleMovement();
            DetectInteractable();
            HandleInteraction();
        }

        private void HandleMovement()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            Vector3 movement = new Vector3(horizontal, 0f, vertical);

            if (movement.sqrMagnitude > 1f)
            {
                movement.Normalize();
            }

            transform.position += movement * moveSpeed * Time.deltaTime;

            if (movement.sqrMagnitude > 0.01f)
            {
                transform.forward = movement;
            }
        }

        private void DetectInteractable()
        {
            currentInteractable = null;

            Collider[] colliders = Physics.OverlapSphere(
                transform.position,
                interactionRange,
                interactableLayer
            );

            float closestDistance = float.MaxValue;

            foreach (Collider collider in colliders)
            {
                IInteractable interactable =
                    collider.GetComponentInParent<IInteractable>();

                if (interactable == null)
                {
                    continue;
                }

                float distance =
                    Vector3.Distance(transform.position, collider.transform.position);

                if (distance < closestDistance &&
                    interactable.CanInteract(this))
                {
                    closestDistance = distance;
                    currentInteractable = interactable;
                }
            }
        }

        private void HandleInteraction()
        {
            if (currentInteractable == null)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                currentInteractable.Interact(this);
            }
        }

        public PlayerInventory Inventory => inventory;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, interactionRange);
        }
    }
}
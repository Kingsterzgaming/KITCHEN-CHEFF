using UnityEngine;
using YesChef.Core;
using YesChef.Stations;

namespace YesChef.Player
{
    [RequireComponent(typeof(PlayerInventory))]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInputHandler))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 5f;

        [Header("Interaction")]
        [SerializeField] private float interactionRange = 1.5f;
        [SerializeField] private LayerMask interactableLayer;

        [Header("Session")]
        [SerializeField] private GameSession gameSession;

        private PlayerInventory inventory;
        private CharacterController characterController;
        private PlayerInputHandler inputHandler;

        private IInteractable currentInteractable;

        public PlayerInventory Inventory => inventory;

        public bool HasInteractable =>
            currentInteractable != null;

        public IInteractable CurrentInteractable =>
            currentInteractable;

        private void Awake()
        {
            inventory = GetComponent<PlayerInventory>();
            characterController = GetComponent<CharacterController>();
            inputHandler = GetComponent<PlayerInputHandler>();
        }

        private void Update()
        {
            if (gameSession != null &&
                !gameSession.IsPlaying)
            {
                currentInteractable = null;
                return;
            }

            HandleMovement();
            DetectInteractable();
            HandleInteraction();
        }

        //private void HandleMovement()
        //{
        //    Vector2 input = inputHandler.MoveInput;

        //    Vector3 movement =
        //        new Vector3(
        //            input.x,
        //            0f,
        //            input.y
        //        );

        //    if (movement.sqrMagnitude > 1f)
        //        movement.Normalize();

        //    characterController.Move(
        //        movement *
        //        moveSpeed *
        //        Time.deltaTime
        //    );

        //    if (movement.sqrMagnitude > 0.01f)
        //    {
        //        transform.forward = movement;
        //    }
        //}
        [Header("Gravity")]
        [SerializeField] private float gravity = -20f;

        private float verticalVelocity;

        private void HandleMovement()
        {
            Vector2 input = inputHandler.MoveInput;

            Vector3 movement =
                new Vector3(
                    input.x,
                    0f,
                    input.y
                );

            if (movement.sqrMagnitude > 1f)
                movement.Normalize();

            if (characterController.isGrounded)
            {
                verticalVelocity = -2f;
            }
            else
            {
                verticalVelocity += gravity * Time.deltaTime;
            }

            movement.y = verticalVelocity;

            characterController.Move(
                movement * Time.deltaTime
            );

            if (movement.x != 0f || movement.z != 0f)
            {
                Vector3 facingDirection =
                    new Vector3(
                        movement.x,
                        0f,
                        movement.z
                    );

                transform.forward =
                    facingDirection.normalized;
            }
        }



        private void DetectInteractable()
        {
            currentInteractable = null;

            Collider[] colliders =
                Physics.OverlapSphere(
                    transform.position,
                    interactionRange,
                    interactableLayer
                );

            float closestDistance =
                float.MaxValue;

            foreach (Collider collider in colliders)
            {
                IInteractable interactable =
                    collider.GetComponentInParent<IInteractable>();

                if (interactable == null)
                    continue;

                float distance =
                    Vector3.Distance(
                        transform.position,
                        collider.ClosestPoint(
                            transform.position
                        )
                    );

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
                return;

            if (inputHandler.ConsumeInteractPressed())
            {
                currentInteractable.Interact(this);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;

            Gizmos.DrawWireSphere(
                transform.position,
                interactionRange
            );
        }
    }
}
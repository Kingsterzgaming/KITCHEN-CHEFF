using UnityEngine;
using UnityEngine.InputSystem;

namespace YesChef.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [Header("Input Actions")]
        [SerializeField] private InputActionReference moveAction;
        [SerializeField] private InputActionReference interactAction;
        [SerializeField] private InputActionReference pauseAction;

        public Vector2 MoveInput
        {
            get
            {
                if (moveAction == null)
                    return Vector2.zero;

                return moveAction.action.ReadValue<Vector2>();
            }
        }

        private void OnEnable()
        {
            EnableAction(moveAction);
            EnableAction(interactAction);
            EnableAction(pauseAction);
        }

        private void OnDisable()
        {
            DisableAction(moveAction);
            DisableAction(interactAction);
            DisableAction(pauseAction);
        }

        public bool ConsumeInteractPressed()
        {
            if (interactAction == null)
                return false;

            return interactAction.action.WasPressedThisFrame();
        }

        public bool ConsumePausePressed()
        {
            if (pauseAction == null)
                return false;

            return pauseAction.action.WasPressedThisFrame();
        }

        private void EnableAction(
            InputActionReference actionReference)
        {
            if (actionReference != null)
                actionReference.action.Enable();
        }

        private void DisableAction(
            InputActionReference actionReference)
        {
            if (actionReference != null)
                actionReference.action.Disable();
        }
    }
}
// #define DEBUG_PLAYER_CONTROLLER

using UnityEngine;
using UnityEngine.InputSystem;

namespace Character.Controller
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : ControllerBase
    {
        // The input vector must be at least bigger than the specified value to be considered valid. 
        [SerializeField] private float impulseTolerance = 0.3f;

        protected override void OnEnable()
        {
            base.OnEnable();
            EnableInput();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            DisableInput();
        }
        protected override void OnCharacterDeath(GameCharacter controlledCharacter)
        {
            base.OnCharacterDeath(controlledCharacter);
            DisableInput();
        }

        private void OnPerformMove(InputAction.CallbackContext context)
        {
            var moveVec = context.ReadValue<Vector2>();
            if (moveVec.magnitude <= impulseTolerance)
                return;
            
            ControlledCharacter.Movement.MoveVector = moveVec;
        #if DEBUG_PLAYER_CONTROLLER
            Debug.Log($"Performed Move={m_movement.MoveVector} Size={m_movement.MoveVector.magnitude}");                
        #endif
        }

        private void OnCanceledMove(InputAction.CallbackContext context)
        {
            ControlledCharacter.Movement.MoveVector = Vector2.zero;
        #if DEBUG_PLAYER_CONTROLLER
            Debug.Log("Canceled");
        #endif
        }

        private void EnableInput()
        {
            var playerInput = GetComponent<PlayerInput>();
            var inputAction = playerInput.actions["Move"];
            inputAction.performed += OnPerformMove;
            inputAction.canceled += OnCanceledMove;
            playerInput.enabled = true;
        }
        
        private void DisableInput()
        {
            var playerInput = GetComponent<PlayerInput>();
            var inputAction = playerInput.actions["Move"];
            inputAction.performed -= OnPerformMove;
            inputAction.canceled -= OnCanceledMove;
            playerInput.enabled = true;
            
            if (ControlledCharacter != null)
                ControlledCharacter.Movement.MoveVector = Vector2.zero;
        }
    }
}

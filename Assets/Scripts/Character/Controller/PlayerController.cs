// #define DEBUG_PLAYER_CONTROLLER

using Character.Behaviour;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace Character.Controller
{
    [RequireComponent(typeof(Movement))]
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : ControllerBase
    {
        // The input vector must be at least bigger than the specified value to be considered valid. 
        [SerializeField] private float impulseTolerance = 0.1f;
        
        private Movement m_movement;

        protected override void Start()
        {
            base.Start();
            
            m_movement = GetComponent<Movement>();

            var playerInput = GetComponent<PlayerInput>();
            Assert.IsNotNull(playerInput);
            
            // Initializes input actions
            var moveAction = playerInput.actions["Move"];
            Assert.IsNotNull(moveAction);
            moveAction.performed += OnPerformMove;
            moveAction.canceled += OnCanceledMove;
        }

        private void OnPerformMove(InputAction.CallbackContext context)
        {
            var moveVec = context.ReadValue<Vector2>();
            if (moveVec.magnitude <= impulseTolerance)
                return;
            
            m_movement.MoveVector = moveVec;
        #if DEBUG_PLAYER_CONTROLLER
            Debug.Log($"Performed Move={m_movement.MoveVector} Size={m_movement.MoveVector.magnitude}");                
        #endif
        }

        private void OnCanceledMove(InputAction.CallbackContext context)
        {
            m_movement.MoveVector = Vector2.zero;
        #if DEBUG_PLAYER_CONTROLLER
            Debug.Log("Canceled");
        #endif
        }
    }
}

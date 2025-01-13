// #define DEBUG_PLAYER_CONTROLLER

using System;
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
        private InputAction m_moveAction;

        protected override void Awake()
        {
            base.Awake();
            
            m_movement = GetComponent<Movement>();

            var playerInput = GetComponent<PlayerInput>();
            Assert.IsNotNull(playerInput);
            
            // Initializes input actions
            m_moveAction = playerInput.actions["Move"];
            Assert.IsNotNull(m_moveAction);
        }

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

        private void EnableInput()
        {
            m_moveAction.performed += OnPerformMove;
            m_moveAction.canceled += OnCanceledMove;
        }
        
        private void DisableInput()
        {
            m_moveAction.performed -= OnPerformMove;
            m_moveAction.canceled -= OnCanceledMove;
            m_movement.MoveVector = Vector2.zero;
        }
    }
}

// #define DEBUG_PLAYER_CONTROLLER

using Character.Behaviour;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Character.Controller
{
    [RequireComponent(typeof(Movement))]
    public class PlayerController : ControllerBase
    {
        // The input vector must be at least bigger than the specified value to be considered valid. 
        [SerializeField] private float impulseTolerance = 0.3f;
        
        private Movement m_movement;

        private void Start()
        {
            m_movement = GetComponent<Movement>();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                var moveVec = context.ReadValue<Vector2>();
                if (moveVec.magnitude <= impulseTolerance)
                    return;
                
                m_movement.MoveVector = moveVec;
            #if DEBUG_PLAYER_CONTROLLER
                Debug.Log($"Performed Move={m_movement.MoveVector} Size={m_movement.MoveVector.magnitude}");                
            #endif
            }
            else if (context.canceled)
            {
                m_movement.MoveVector = Vector2.zero;
            #if DEBUG_PLAYER_CONTROLLER
                Debug.Log("Canceled");
            #endif
            }
        }
    }
}

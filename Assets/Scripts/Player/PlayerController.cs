using System;
using Character;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Movement))]
    public class PlayerController : MonoBehaviour
    {
        // The input vector must be at least bigger than the specified value to be considered valid. 
        [SerializeField] private float impulseTolerance = 0.3f;
        
        private static readonly int Attack = Animator.StringToHash("Attack");
        private Movement m_movement;

        private void Start()
        {
            m_movement = GetComponent<Character.Movement>();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                var moveVec = context.ReadValue<Vector2>();
                if (moveVec.magnitude <= impulseTolerance)
                    return;
                
                m_movement.MoveVector = moveVec;
                Debug.Log($"Performed Move={m_movement.MoveVector} Size={m_movement.MoveVector.magnitude}");
            }
            else if (context.canceled)
            {
                m_movement.MoveVector = Vector2.zero;
                Debug.Log("Canceled");
            }
        }

        public void HandleAttack(InputAction.CallbackContext context)
        {
            var animator = GetComponent<Animator>();
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                return;
            }
            
            animator.SetTrigger(Attack);
        }
    }
}

using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        // Move speed in unit/second.
        public float moveSpeed = 2;
        private void Update()
        {
            if (_isMoving)
            {
                transform.Translate(_moveVector * (moveSpeed * Time.deltaTime));
            }
        }

        private bool _isMoving = false; 
        private Vector2 _moveVector;
        private static readonly int Attack = Animator.StringToHash("Attack");

        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _moveVector = context.ReadValue<Vector2>();
                // Debug.Log("Performed");
            }
            else if (context.started)
            {
                _isMoving = true;
                // Debug.Log("Started");
            }
            else if (context.canceled)
            {
                _isMoving = false;
                // Debug.Log("Canceled");
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

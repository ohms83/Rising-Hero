using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Test.Scripts
{
    public class TestCharacter : MonoBehaviour
    {
        private Vector2 m_moveVec;
        private Rigidbody2D m_rigidBody2D;
        public bool useRigidBody = true;

        private void Start()
        {
            m_rigidBody2D = GetComponent<Rigidbody2D>();

            var playerInput = GetComponent<PlayerInput>();
            if (!playerInput) return;

            playerInput.actions["Move"].performed += HandleMove;
            playerInput.actions["Move"].canceled += HandleMove;
        }

        private void HandleMove(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                m_moveVec = context.ReadValue<Vector2>();
            }
            else if (context.canceled)
            {
                m_moveVec = Vector2.zero;
            }
        }
        private void Update()
        {
            if (useRigidBody)
                m_rigidBody2D.linearVelocity = m_moveVec * 2;
            else
                transform.Translate(m_moveVec * (Time.deltaTime * 2));
        }
    }
}

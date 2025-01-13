using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Character.Behaviour
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Movement : MonoBehaviour
    {
        // Move speed in unit/second.
        [SerializeField] private float moveSpeed = 2;
        [SerializeField] private CharacterAnimation characterAnimation;

        public float MoveSpeed
        {
            get => moveSpeed;
            set => moveSpeed = value;
        }

        public Vector2 MoveVector
        {
            get => m_moveVector;
            set
            {
                m_moveVector = value;
                m_isMoving = value != Vector2.zero;
            
                if (characterAnimation ? characterAnimation : null)
                {
                    characterAnimation.SetRunningFlag(m_isMoving);
                }

                m_rigidbody2D.linearVelocity = value * MoveSpeed;
            }
        }
        private Vector2 m_moveVector;
        
        private bool m_isMoving;
        private Rigidbody2D m_rigidbody2D;
        private Collider2D m_collider2D;

        private void Awake()
        {
            m_rigidbody2D = GetComponent<Rigidbody2D>();
            Assert.IsNotNull(m_rigidbody2D);

            m_rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            m_rigidbody2D.useFullKinematicContacts = true;
            m_rigidbody2D.freezeRotation = true;
        }
    }
}

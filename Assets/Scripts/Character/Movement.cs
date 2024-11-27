using UnityEngine;

namespace Character
{
    public class Movement : MonoBehaviour
    {
        // Move speed in unit/second.
        [SerializeField]
        private float moveSpeed = 2;

        public Vector2 MoveVector
        {
            get => m_moveVector;
            set
            {
                m_moveVector = value;
                m_isMoving = value != Vector2.zero;

                if (value.x != 0 && m_spriteRenderer != null) 
                    m_spriteRenderer.flipX = m_moveVector.x < 0;
            }
        }
        private Vector2 m_moveVector;
        
        private bool m_isMoving = false;
        private SpriteRenderer m_spriteRenderer;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            m_spriteRenderer = GetComponent<SpriteRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            if (m_isMoving)
            {
                transform.Translate(MoveVector * (moveSpeed * Time.deltaTime));
            }
        }
    }
}

using UnityEngine;

namespace Character.Behaviour
{
    public class Movement : MonoBehaviour
    {
        // Move speed in unit/second.
        [SerializeField]
        private float moveSpeed = 2;
        [SerializeField] private Animator animator;

        public Vector2 MoveVector
        {
            get => m_moveVector;
            set
            {
                m_moveVector = value;
                m_isMoving = value != Vector2.zero;
            
                if (animator ? animator : null)
                {
                    animator.SetBool(RunningFlag, m_isMoving);
                }
            }
        }
        private Vector2 m_moveVector;
        
        private bool m_isMoving;
        private SpriteRenderer m_spriteRenderer;
        
        private static readonly int RunningFlag = Animator.StringToHash("IsRunning");
        
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

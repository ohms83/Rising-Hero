using Character.Behaviour;
using UnityEngine;
using UnityEngine.Assertions;

namespace Character
{
    [RequireComponent(typeof(Movement))]
    public class GameCharacter : MonoBehaviour
    {
        public SpriteRenderer characterSprite;
        
        private Movement m_movement;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            m_movement = GetComponent<Movement>();
            Assert.IsNotNull(m_movement);
        }

        // Update is called once per frame
        void Update()
        {
            var moveX = m_movement.MoveVector.x;
            if (moveX != 0 && !ReferenceEquals(characterSprite, null))
            {
                // m_spriteRenderer.flipX = moveX < 0;
                var yaw = moveX < 0 ? 180f : 0f;
                characterSprite.transform.eulerAngles = new Vector3(0, yaw, 0);
            }
        }
    }
}

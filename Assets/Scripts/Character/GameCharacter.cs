using System.Collections.Generic;
using Character.Behaviour;
using Skills;
using UnityEngine;
using UnityEngine.Assertions;

namespace Character
{
    [RequireComponent(typeof(Movement))]
    public class GameCharacter : MonoBehaviour
    {
        [SerializeField] private Stats stats;
        [SerializeField] private SpriteRenderer characterSprite;

        public Stats Stats => stats;

        #region Skill
        [SerializeField] private List<SkillType> skillTypes; 
        protected readonly HashSet<SkillBase> Skills = new();
        #endregion

        private Movement m_movement;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected virtual void Start()
        {
            m_movement = GetComponent<Movement>();
            Assert.IsNotNull(m_movement);
            
            foreach (var skillType in skillTypes)
            {
                Skills.Add(SkillFactory.CreateSkill(skillType, this));
            }
        }

        // Update is called once per frame
        private void Update()
        {
            var moveX = m_movement.MoveVector.x;
            if (moveX != 0 && !ReferenceEquals(characterSprite, null))
            {
                var yaw = moveX < 0 ? 180f : 0f;
                characterSprite.transform.eulerAngles = new Vector3(0, yaw, 0);
            }
        }

        public void TakeDamage(int damage)
        {
            if (stats.IsDeath)
                return;
            stats.Health -= damage;
        }
    }
}

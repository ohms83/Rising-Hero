using System.Collections.Generic;
using System.Linq;
using Character.Behaviour;
using Gameplay;
using Gameplay.Equipment;
using Skills;
using UnityEngine;
using UnityEngine.Assertions;

namespace Character
{
    [RequireComponent(typeof(Movement))]
    public class GameCharacter : MonoBehaviour, IEquipable
    {
        [SerializeField] private Stats stats;
        [SerializeField] private SpriteRenderer characterSprite;
        [SerializeField] private CharacterAnimation characterAnimation;
        public SpriteRenderer CharacterSprite => characterSprite;
        public CharacterAnimation CharacterAnimation => characterAnimation;

        public Stats Stats => stats;

        #region Skill
        [SerializeField] private List<SkillType> skillTypes; 
        protected readonly HashSet<SkillBase> Skills = new();
        #endregion

        #region Equipment
        private readonly Dictionary<EquipmentType, Equipment> m_equipments = new ();
        
        public Stats CombinedStats { get; private set; }
        public void Equip(Equipment newEquipment)
        {
            m_equipments.Add(newEquipment.Type, newEquipment);

            CombinedStats = Stats;
            foreach (var equipment in m_equipments.Values)
            {
                CombinedStats += equipment.Stats;

                if (SkillBase.IsValidSkillType(equipment.Skill))
                    EquipSkill(equipment.Skill);
            }

            newEquipment.Owner = this;
        }
        #endregion

        public void TakeDamage(int damage)
        {
            if (stats.IsDeath)
                return;
            stats.Health -= damage;
        }

        public void EquipSkill(SkillType skillType)
        {
            var skill = SkillFactory.CreateSkill(skillType, this);
            if (skill != null)
                Skills.Add(skill);
        }

        private Movement m_movement;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected virtual void Start()
        {
            m_movement = GetComponent<Movement>();
            Assert.IsNotNull(m_movement);
            
            foreach (var skill in skillTypes
                         .Select(skillType => SkillFactory.CreateSkill(skillType, this))
                         .Where(skill => skill != null))
            {
                Skills.Add(skill);
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
    }
}

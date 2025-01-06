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
        [SerializeField] private bool autoCastAllSkills = false; 
        private readonly HashSet<SkillBase> Skills = new();
        #endregion

        #region Equipment
        [SerializeField] private List<Equipment> defaultEquipments = new();
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
                    EquipSkillType(equipment.Skill);
            }

            newEquipment.Owner = this;
            newEquipment.gameObject.layer = gameObject.layer;

        }
        #endregion

        public void TakeDamage(int damage)
        {
            if (stats.IsDeath)
                return;
            
            stats.Health -= damage;
        }

        /// <summary>
        /// Equip a skill from the specified skill type
        /// </summary>
        /// <param name="skillType">A SkillType enum indicating the equipping skill</param>
        private void EquipSkillType(SkillType skillType)
        {
            EquipSkill(SkillFactory.CreateSkill(skillType, this));
        }
        
        protected virtual void EquipSkill(SkillBase skill)
        {
            if (skill == null)
                return;
            
            Skills.Add(skill);

            if (autoCastAllSkills)
                skill.IsAutoCast = true;
        }

        private Movement m_movement;
        private DeathBehaviour m_deathBehaviour;

        protected void Awake()
        {
            CombinedStats = Stats;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected virtual void Start()
        {
            m_movement = GetComponent<Movement>();
            Assert.IsNotNull(m_movement);
            
            foreach (var skill in skillTypes
                         .Select(skillType => SkillFactory.CreateSkill(skillType, this))
                         .Where(skill => skill != null))
            {
                EquipSkill(skill);
            }

            foreach (var equipment in defaultEquipments)
            {
                Equip(Instantiate(equipment));
            }
            
            stats.Reset();
            
            m_deathBehaviour = GetComponent<DeathBehaviour>();
            if (m_deathBehaviour == null)
                Debug.LogAssertion($"{gameObject} has no DeathBehaviour attache");
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
            
            if (stats.IsDeath && !m_deathBehaviour.IsDeathSequenceStarted)
                m_deathBehaviour.BeginDeathSequence();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Character.Behaviour;
using Character.Controller;
using Gameplay;
using Gameplay.Equipment;
using ScriptableObjects.Character;
using Skills;
using UnityEngine;

namespace Character
{
    [RequireComponent(typeof(Movement))]
    public class GameCharacter : MonoBehaviour, IEquipable
    {
        [SerializeField] private Stats stats;
        [Tooltip("A shared and immutable data containing crucial information about the character--sprite, animation, stats, etc.")]
        [SerializeField] private GameCharacterData sharedData;
        [SerializeField] private SpriteRenderer characterSprite;
        [SerializeField] private CharacterAnimation characterAnimation;
        public SpriteRenderer CharacterSprite => characterSprite;
        public CharacterAnimation CharacterAnimation => characterAnimation;
        public Stats Stats => stats;
        public GameCharacterData SharedData => sharedData;

        public ControllerBase Controller
        {
            get;
            private set;
        }

        #region Skill
        
        [SerializeField] private List<SkillType> skillTypes;
        [SerializeField] private bool autoCastAllSkills = false; 
        private readonly HashSet<SkillBase> m_skills = new();
        
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

        #region Event
        
        public Action<GameCharacter> onCharacterDestroyed;

        #endregion

        protected void Awake()
        {
            CombinedStats = Stats;
            Controller = GetComponent<ControllerBase>();
            Movement = GetComponent<Movement>();
            
            m_deathBehaviour = GetComponent<DeathBehaviour>();
            if (m_deathBehaviour == null)
                Debug.LogAssertion($"{gameObject} has no DeathBehaviour attache");
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected virtual void Start()
        {
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
        }

        // Update is called once per frame
        private void Update()
        {
            var moveX = Movement.MoveVector.x;
            if (moveX != 0 && !ReferenceEquals(characterSprite, null))
            {
                var yaw = moveX < 0 ? 180f : 0f;
                characterSprite.transform.eulerAngles = new Vector3(0, yaw, 0);
            }

            if (stats.IsDeath && !m_deathBehaviour.IsDeathSequenceStarted)
            {
                m_deathBehaviour.BeginDeathSequence();
                Movement.MoveVector = Vector2.zero;
            }
        }

        private void OnDestroy()
        {
            onCharacterDestroyed?.Invoke(this);
        }

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
        
        private void EquipSkill(SkillBase skill)
        {
            if (skill == null)
                return;
            
            m_skills.Add(skill);

            if (autoCastAllSkills)
                skill.IsAutoCast = true;
        }
        
        public Movement Movement
        {
            get;
            private set;
        }
        private DeathBehaviour m_deathBehaviour;
    }
}

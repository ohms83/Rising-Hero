using System;
using System.Collections.Generic;
using System.Linq;
using Character.Behaviour;
using Character.Controller;
using Gameplay;
using Gameplay.Equipment;
using Pattern;
using ScriptableObjects.Character;
using ScriptableObjects.Event;
using Skills;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Character
{
    [RequireComponent(typeof(Movement))]
    public class GameCharacter : MonoBehaviour, IEquipable
    {
        public ValueEvent<int> health;
        public bool IsDeath => health.Value <= 0;
        
        [SerializeField] private Stats stats;
        [Tooltip("A shared and immutable data containing crucial information about the character--sprite, animation, stats, etc.")]
        [SerializeField] private GameCharacterData sharedData;
        [SerializeField] private SpriteRenderer characterSprite;
        [SerializeField] private CharacterAnimation characterAnimation;
        // public SpriteRenderer CharacterSprite => characterSprite;
        public CharacterAnimation CharacterAnimation => characterAnimation;
        public Stats Stats => stats;
        public GameCharacterData SharedData => sharedData;

        public ControllerBase Controller
        {
            get;
            private set;
        }

        #region Skill
        
        [Tooltip("If set, all the equipped skills will be auto-cast.")]
        [SerializeField] private bool m_autoCastAllSkills;

        public void SetAutoCast(bool flag)
        {
            foreach (var skill in Skills)
            {
                skill.IsAutoCast = flag;
            }
            m_autoCastAllSkills = flag;
        }
        
        private readonly HashSet<SkillBase> m_skills = new();
        public HashSet<SkillBase> Skills => m_skills;
        
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
                    EquipSkillType(equipment.Skill);
            }

            newEquipment.Owner = this;
            newEquipment.gameObject.layer = gameObject.layer;
        }
        
        #endregion

        #region Events
        
        [Header("Events")]
        public UnityEvent<GameCharacter> onCharacterDestroyed;
        public UnityEvent<GameCharacter> onCharacterDeath;
        
        [SerializeField] private CharacterEvent m_characterSpawnedEvent;
        [SerializeField] private CharacterEvent m_characterDeathEvent;

        #endregion
        
        public Movement Movement
        {
            get;
            private set;
        }
        private DeathBehaviour m_deathBehaviour;

        protected void Awake()
        {
            CombinedStats = Stats;
            
            Controller = GetComponent<ControllerBase>();
            Movement = GetComponent<Movement>();
            
            m_deathBehaviour = GetComponent<DeathBehaviour>();
            if (m_deathBehaviour == null)
                Debug.LogAssertion($"{gameObject} has no DeathBehaviour attache");
            
            if (m_characterSpawnedEvent != null)
                m_characterSpawnedEvent.onEventRaised?.Invoke(this);
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected virtual void Start()
        {
            InitCharacterData();
            health.onValueChanged.AddListener(OnHealthUpdated);
            health.Value = CombinedStats.MaxHealth;
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
        }

        private void OnDestroy()
        {
            onCharacterDestroyed?.Invoke(this);
        }

        private void InitCharacterData()
        {
            if (sharedData == null)
                return;
            
            foreach (var skill in sharedData.defaultSkills
                         .Select(skillType => SkillFactory.CreateSkill(skillType, this))
                         .Where(skill => skill != null))
            {
                EquipSkill(skill);
            }

            foreach (var equipment in sharedData.defaultEquipments
                         .Select(Instantiate)
                         .Where(equipment => equipment != null))
            {
                Equip(equipment);
            }

            Movement.MoveSpeed = stats.MoveSpeed;
        }

        public void TakeDamage(int damage)
        {
            if (IsDeath)
                return;

            var newValue = health.Value - damage;
            health.Value = newValue < 0 ? 0 : newValue;
        }

        private void OnHealthUpdated(int oldValue, int newValue)
        {
            if (newValue <= 0)
            {
                OnDeath();
            }
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

            if (m_autoCastAllSkills)
                skill.IsAutoCast = true;
        }

        private void OnDeath()
        {
            foreach (var keyValue in m_equipments)
            {
                keyValue.Value.enabled = false;
            }

            enabled = false;
            Movement.MoveVector = Vector2.zero;

            m_deathBehaviour.BeginDeathSequence();
            onCharacterDeath?.Invoke(this);
            
            if (m_characterDeathEvent != null)
                m_characterDeathEvent.onEventRaised?.Invoke(this);
        }

        private void OnGUI()
        {
            SetAutoCast(m_autoCastAllSkills);
        }
    }
}

using System;
using System.Collections.Generic;
using Character;
using UnityEngine;

namespace Skills
{
    [RequireComponent(typeof(GameCharacter))]
    public class SkillSystem : MonoBehaviour
    {
        [SerializeField] private List<SkillEffect> m_defaultSkills;
        [Tooltip("If set, all the equipped skills will be auto-cast.")]
        [SerializeField] private bool m_autoCast;

        private GameCharacter m_owner;
        private readonly Dictionary<SkillType, SkillBase> m_equippedSkills = new ();
        
        #if UNITY_EDITOR
        /// <summary>
        /// A cached value of m_autoCast using mainly by the OnGUI function.
        /// </summary>
        private bool m_uiAutoCast; 
        #endif

        private void Awake()
        {
            m_owner = GetComponent<GameCharacter>();
            EquipSkills(m_defaultSkills);
        }

        private void Start()
        {
            SetAutoCast(m_autoCast);
        }

        public void EquipSkills(IEnumerable<SkillEffect> skillList)
        {
            foreach (var skill in skillList)
            {
                EquipSkill(skill);
            }
        }

        public SkillBase EquipSkill(SkillEffect skillEffect)
        {
            foreach (var removeSkillType in skillEffect.data.replaceEffects)
            {
                if (!m_equippedSkills.TryGetValue(removeSkillType, out var removeSkill))
                    continue;
                removeSkill.Deactivate();
                m_equippedSkills.Remove(removeSkillType);
            }

            var newSkill = new SkillBase(m_owner, skillEffect);
            m_equippedSkills.Add(skillEffect.data.skillType, newSkill);
            
            if (skillEffect.data.activeCondition == ActiveCondition.Passive)
                newSkill.Activate();

            return newSkill;
        }

        public bool IsActive(SkillType skillType)
        {
            return m_equippedSkills.TryGetValue(skillType, out var skillEffect) && skillEffect.IsActive;
        }

        public bool IsCooldown(SkillType skillType)
        {
            return m_equippedSkills.TryGetValue(skillType, out var skillEffect) && skillEffect.IsCooldown;
        }

        public SkillBase ActivateSkill(SkillType skillType)
        {
            if (!m_equippedSkills.TryGetValue(skillType, out var skill))
            {
                Debug.LogWarning($"Skill {skillType} is not equipped by {gameObject}");
                return null;
            }

            ActivateSkill(skill);
            return skill;
        }

        private void ActivateSkill(SkillBase skill)
        {
            if (skill.IsActive || skill.IsCooldown)
                return;
            
            skill.onFinishedCooldown += @base =>
            {
                if (m_autoCast)
                    @base.Activate();
            };
            skill.Activate();
        }

        public void SetAutoCast(bool flag)
        {
            m_autoCast = flag;
            if (!m_autoCast)
                return;

            foreach (var keyValue in m_equippedSkills)
            {
                ActivateSkill(keyValue.Value);
            }
        }
        
        #if UNITY_EDITOR
        // private void OnGUI()
        // {
        //     if (m_uiAutoCast == m_autoCast)
        //         return;
        //     m_uiAutoCast = m_autoCast;
        //     SetAutoCast(m_uiAutoCast);
        // }
        #endif
    }
}

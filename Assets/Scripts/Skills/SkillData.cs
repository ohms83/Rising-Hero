using System.Collections.Generic;
using UnityEngine;

namespace Skills
{
    public enum ActiveCondition
    {
        /// <summary>
        /// Skill can only be activated by calling Activate function
        /// and when the cooldown period has ended
        /// </summary>
        Active,
        /// <summary>
        /// Skill's effects are always active.
        /// </summary>
        Passive,
    }
    [CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Objects/Skills/Data")]
    public class SkillData : ScriptableObject
    {
        public string skillName;
        public string description;
        public SkillType skillType;
        public ActiveCondition activeCondition;
        [Tooltip("How long the skill will be effective after activation (in seconds).")]
        public float activeTime;
        [Tooltip("Skill's cooldown period (in seconds).")]
        public float cooldown;
        [Tooltip("A list of skills that will be removed once this skill is activated.")]
        public List<SkillType> replaceEffects;
    }
}

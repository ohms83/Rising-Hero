using System.Collections.Generic;
using UnityEngine;

namespace Skills
{
    public static class SkillFactory
    {
        private static readonly Dictionary<SkillType, ISkillCreator> CreatorDict = new()
        {
            { SkillType.MeleeAttack , new MeleeAttackCreator() }
        };
        
        public static SkillBase CreateSkill(SkillType type, MonoBehaviour owner)
        {
            return CreatorDict.TryGetValue(type, out var creator) ? creator.CreateSkill(owner) : null;
        }
    }
}

using System;
using System.Collections.Generic;
using Character;
using Skills;
using UnityEngine;
using Utils.Collection;

namespace Gameplay.System.LevelUp
{
    [Serializable]
    public struct LevelUpReward
    {
        public string name;
        public string description;
        public Stats stats;
        public SkillEffect skill;

        public void ApplyReward(GameCharacter character)
        {
            character.Stats += stats;
            
            if (skill == null)
                return;

            character.SkillSystem.EquipSkill(skill);
        }
    }
    
    [CreateAssetMenu(fileName = "LevelUpRewardPool", menuName = "Scriptable Objects/Gameplay/Level Up/LevelUpRewardPool")]
    public class LevelUpRewardPool : ScriptableObject
    {
        public List<LevelUpReward> rewards;

        public List<LevelUpReward> GetRandomRewards(int num)
        {
            return RandomizeList<LevelUpReward>.GetRandomItems(rewards, num);
        }
    }
}

using Character;
using UnityEngine;

namespace Skills
{
    public class MeleeAttack : SkillBase
    {
        private readonly CharacterAnimation m_animator;
        
        public MeleeAttack(MonoBehaviour owner) : base(owner)
        {
            var gameCharacter = (GameCharacter)owner;
            m_animator = gameCharacter? gameCharacter.CharacterAnimation : null;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        protected override void ActivateInternal()
        {
            if (m_animator != null)
                m_animator.Attack();
            else
                Debug.LogWarning("m_animator is not set.");
        }
    }
    
    public class MeleeAttackCreator : ISkillCreator
    {
        public SkillBase CreateSkill(MonoBehaviour owner)
        {
            return new MeleeAttack(owner);
        }
    }
}

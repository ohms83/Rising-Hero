using UnityEngine;

namespace Skills
{
    public class MeleeAttack : SkillBase
    {
        private static readonly int AttackTrigger = Animator.StringToHash("Attack");
        private readonly Animator m_animator;
        
        public MeleeAttack(MonoBehaviour owner) : base(owner)
        {
            m_animator = owner.GetComponentInChildren<Animator>();
        }

        // ReSharper disable Unity.PerformanceAnalysis
        protected override void ActivateInternal()
        {
            if (m_animator != null)
                m_animator.SetTrigger(AttackTrigger);
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

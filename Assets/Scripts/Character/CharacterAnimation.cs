using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Character
{
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimation : MonoBehaviour
    {
        public delegate void AnimationEventBool(CharacterAnimation sender, bool value);
        public AnimationEventBool HitBoxAnimationEvent;
        
        private static readonly int AttackHash = Animator.StringToHash("Attack");
        private static readonly int DeathHash = Animator.StringToHash("Death");
        private static readonly int IsDeathHash = Animator.StringToHash("IsDeath");
        private static readonly int IsRunningHash = Animator.StringToHash("IsRunning");
        
        private Animator m_animator;
        private void Start()
        {
            m_animator = GetComponent<Animator>();
            Assert.IsNotNull(m_animator);
        }

        /// <summary>
        /// Play attack animation.
        /// </summary>
        public void Attack()
        {
            m_animator.SetTrigger(AttackHash);
        }

        public void SetRunningFlag(bool flag)
        {
            m_animator.SetBool(IsRunningHash, flag);
        }

        public void Death()
        {
            m_animator.SetBool(IsDeathHash, true);
            m_animator.SetTrigger(DeathHash);
        }

        private void OnHitBoxEnabled()
        {
            HitBoxAnimationEvent?.Invoke(this, true);
        }
        
        private void OnHitBoxDisabled()
        {
            HitBoxAnimationEvent?.Invoke(this, false);
        }
    }
}
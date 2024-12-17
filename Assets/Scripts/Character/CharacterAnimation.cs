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
        
        private static readonly int AttackTrigger = Animator.StringToHash("Attack");
        
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
            m_animator.SetTrigger(AttackTrigger);
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
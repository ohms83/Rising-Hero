using System;
using System.Collections.Generic;
using Character;
using UnityEngine.InputSystem;

namespace Skills
{
    public class SkillBase
    {
        public Action<SkillBase> onFinishedActivation;
        public Action<SkillBase> onFinishedCooldown;

        private readonly GameCharacter m_owner;
        private readonly SkillEffect m_effect;
        
        public bool IsActive
        {
            get;
            private set;
        }
        public bool IsCooldown
        {
            get;
            private set;
        }

        public GameCharacter Owner => m_owner;

        public SkillBase(GameCharacter owner, SkillEffect effect)
        {
            m_owner = owner;
            m_effect = effect;
        }

        public void Activate()
        {
            MEC.Timing.RunCoroutine(ActivateSkillEffect(m_effect));
        }

        public void Deactivate()
        {
            m_effect.Deactivate(m_owner);
        }

        private IEnumerator<float> ActivateSkillEffect(SkillEffect effect)
        {
            // Activate
            yield return MEC.Timing.WaitUntilDone(MEC.Timing.RunCoroutine(ActivationCoroutine(effect)));
            // Cooldown
            yield return MEC.Timing.WaitUntilDone(MEC.Timing.RunCoroutine(CooldownCoroutine(effect)));
        }

        private IEnumerator<float> ActivationCoroutine(SkillEffect effect)
        {
            effect.Activate(m_owner);
            IsActive = true;
            yield return MEC.Timing.WaitForSeconds(effect.data.activeTime);

            // Passive skills is permanently active .
            if (effect.data.activeCondition == ActiveCondition.Passive)
                yield break;
            
            effect.Deactivate(m_owner);
            IsActive = false;

            yield return MEC.Timing.WaitForOneFrame;
            onFinishedActivation?.Invoke(this);
        }

        private IEnumerator<float> CooldownCoroutine(SkillEffect effect)
        {
            IsCooldown = true;
            
            yield return MEC.Timing.WaitForSeconds(effect.data.cooldown);
            
            IsCooldown = false;
            yield return MEC.Timing.WaitForOneFrame;
            onFinishedCooldown?.Invoke(this);
        }
    }
}

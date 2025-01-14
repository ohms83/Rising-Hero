using System;
using System.Collections;
using UnityEngine;

namespace Skills
{
    public abstract class SkillBase
    {
        public float cooldownTime = 1;
        private bool m_isAutoCast;
        private IEnumerator m_cooldownCoroutine;

        public bool IsAutoCast
        {
            get => m_isAutoCast;
            set
            {
                m_isAutoCast = value;
                // There's no need to stop cooldown since the coroutine will automatically stop
                // when the auto-cast is disabled.
                if (value)
                    BeginCooldown();
            }
        }

        public bool IsCoolDown
        {
            get;
            private set;
        }

        public bool CanActivate => !IsCoolDown && m_owner != null;
        
        public delegate void SkillCallback(SkillBase skill);
        public SkillCallback onFinishedCooldown;
        public SkillCallback onSkillActivated;

        private readonly MonoBehaviour m_owner;

        protected SkillBase(MonoBehaviour owner)
        {
            m_owner = owner;
        }

        public void Activate()
        {
            if (!CanActivate)
                return;
            ActivateInternal();
            onSkillActivated?.Invoke(this);
        }
        
        protected abstract void ActivateInternal();

        private void BeginCooldown()
        {
            // ReSharper disable once Unity.PerformanceCriticalCodeNullComparison
            if (m_owner == null || m_cooldownCoroutine != null)
                return;
            m_cooldownCoroutine = CooldownCoroutine();
            m_owner.StartCoroutine(m_cooldownCoroutine);
        }

        private void StopCooldown()
        {
            if (m_owner == null)
                return;
            m_owner.StopCoroutine(m_cooldownCoroutine);
        }

        private IEnumerator CooldownCoroutine()
        {
            while (true)
            {
                IsCoolDown = true;
                yield return new WaitForSeconds(cooldownTime);

                IsCoolDown = false;
                onFinishedCooldown?.Invoke(this);
                
                if (IsAutoCast)
                    Activate();
                else
                    break;
            }

            m_cooldownCoroutine = null;
        }
        
        public static bool IsValidSkillType(SkillType skillType)
        {
            return skillType != SkillType.None && skillType != SkillType.Max;
        }
    }

    public interface ISkillCreator
    {
        public SkillBase CreateSkill(MonoBehaviour owner);
    }
}

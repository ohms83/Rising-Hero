using System;
using System.Collections;
using UnityEngine;

namespace Skills
{
    public abstract class SkillBase
    {
        public float CooldownTime = 1;
        private bool m_isAutoCast = false;

        public bool IsAutoCast
        {
            get => m_isAutoCast;
            set
            {
                m_isAutoCast = value;
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
        public SkillCallback OnFinishedCooldown;
        public SkillCallback OnSkillActivated;

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
            OnSkillActivated?.Invoke(this);
        }
        
        protected abstract void ActivateInternal();

        public void BeginCooldown()
        {
            if (m_owner == null)
                return;
            m_owner.StartCoroutine(CooldownCoroutine());
        }

        private IEnumerator CooldownCoroutine()
        {
            while (true)
            {
                IsCoolDown = true;
                yield return new WaitForSeconds(CooldownTime);

                IsCoolDown = false;
                OnFinishedCooldown?.Invoke(this);
                
                if (IsAutoCast)
                    Activate();
                else
                    break;
            }
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

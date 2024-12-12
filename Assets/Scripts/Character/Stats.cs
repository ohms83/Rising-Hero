using System;
using UnityEngine;

namespace Character
{
    [Serializable]
    public struct Stats
    {
        private int m_health;
        public int Health
        {
            get => m_health;
            set => m_health = Mathf.Clamp(value, 0, MaxHealth);
        }
        
        [SerializeField]
        private int maxHealth;
        public int MaxHealth
        {
            get => maxHealth;
            set => maxHealth = value;
        }

        public bool IsDeath => m_health == 0;

        public void Init()
        {
            Health = MaxHealth;
        }
    }
}

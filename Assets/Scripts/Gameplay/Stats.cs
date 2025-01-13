using System;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    [Serializable]
    public struct Stats
    {
        private int m_health;
        public int Health
        {
            get => m_health;
            set
            {
                if (m_health > 0 && value <= 0)
                {
                    m_health = 0;
                    onDeath?.Invoke();
                }
                else
                {
                    m_health = Mathf.Clamp(value, 0, MaxHealth);
                }
            }
        }
        
        [SerializeField]
        private int maxHealth;
        public int MaxHealth
        {
            get => maxHealth;
            set => maxHealth = value;
        }

        public bool IsDeath => m_health == 0;
        
        [SerializeField]
        private int attack;
        public int Attack
        {
            get => attack;
            set => attack = value;
        }
        
        [SerializeField]
        private int defence;
        public int Defence
        {
            get => defence;
            set => defence = value;
        }
        
        [SerializeField]
        private int moveSpeed;
        public int MoveSpeed
        {
            get => moveSpeed;
            set => moveSpeed = value;
        }
        
        public UnityEvent onDeath;

        /// <summary>
        /// Reset the stats to their initial values
        /// </summary>
        public void Reset()
        {
            Health = MaxHealth;
        }
        
        public static Stats operator + (Stats a, Stats b)
        {
            return new Stats
            {
                attack = a.attack + b.attack,
                defence = a.defence + b.defence,
                maxHealth = a.maxHealth + b.maxHealth,
                moveSpeed = a.moveSpeed + b.moveSpeed,
            };
        }
    }
}

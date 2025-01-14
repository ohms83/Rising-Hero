using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Gameplay
{
    [Serializable]
    public struct Stats
    {
        [SerializeField]
        private int m_maxHealth;
        public int MaxHealth
        {
            get => m_maxHealth;
            set => m_maxHealth = value;
        }
        
        [SerializeField]
        private int m_attack;
        public int Attack
        {
            get => m_attack;
            set => m_attack = value;
        }
        
        [SerializeField]
        private int m_defence;
        public int Defence
        {
            get => m_defence;
            set => m_defence = value;
        }
        
        [SerializeField]
        private float m_moveSpeed;
        public float MoveSpeed
        {
            get => m_moveSpeed;
            set => m_moveSpeed = value;
        }
        
        public static Stats operator + (Stats a, Stats b)
        {
            return new Stats
            {
                m_attack = a.m_attack + b.m_attack,
                m_defence = a.m_defence + b.m_defence,
                m_maxHealth = a.m_maxHealth + b.m_maxHealth,
                m_moveSpeed = a.m_moveSpeed + b.m_moveSpeed,
            };
        }
    }
}

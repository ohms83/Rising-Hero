using System;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    [Serializable]
    public struct Stats
    {
        [SerializeField]
        private int maxHealth;
        public int MaxHealth
        {
            get => maxHealth;
            set => maxHealth = value;
        }
        
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

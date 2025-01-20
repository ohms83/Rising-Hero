using System;
using System.Collections.Generic;
using Character;
using Character.Data;
using Event;
using UnityEngine;
using UnityEngine.Events;
using Utils.Collection;

namespace Gameplay.System.LevelUp
{
    public struct LevelUpEventData
    {
        public int oldLevel;
        public int newLevel;
        public List<LevelUpReward> rewards;
    }
    public class LevelUpSystem : MonoBehaviour
    {
        [SerializeField] private LevelUpData m_levelUpData;
        [Tooltip("An event-bus that will inform the system about the enemy's death.")]
        [SerializeField] private CharacterEvent m_enemyDeathEvent;
        [SerializeField] private List<LevelUpRewardPool> m_rewardPool;
        public UnityEvent<LevelUpEventData> levelUpEvent;

        private void Awake()
        {
            m_enemyDeathEvent.AddListener(OnEnemyDeath);
        }

        private void Start()
        {
            m_levelUpData.Reset();
        }

        private void OnEnemyDeath(GameCharacter enemy)
        {
            if (m_levelUpData == null)
            {
                Debug.LogWarning("Level up data is not configured.");
                return;
            }
            
            var enemyData = (EnemyCharacterData)enemy.SharedData;
            if (enemyData == null)
            {
                Debug.LogWarning($"{enemy}'s SharedData is not an EnemyCharacterData type. Type={enemyData.GetType()}");
                return;
            }

            m_levelUpData.xp += enemyData.xp;
            LevelUpLogic();
        }

        private void LevelUpLogic()
        {
            var level = m_levelUpData.level;
            var hasLeveledUp = m_levelUpData.IsLevelUp;
            
            while (m_levelUpData.IsLevelUp)
            {
                m_levelUpData.xp -= m_levelUpData.ToNextLevel;
                ++m_levelUpData.level;
            }

            if (hasLeveledUp)
            {
                int numItem = 2;
                uint seed = (uint)Time.realtimeSinceStartup;
                levelUpEvent?.Invoke(new LevelUpEventData
                {
                    oldLevel = level,
                    newLevel = m_levelUpData.level,
                    rewards = RandomizeList<LevelUpReward>.GetRandomItems(m_rewardPool[0].rewards, numItem, seed)
                });
                Debug.Log($"LEVEL UP {m_levelUpData.level}");
            }
        }
    }
}

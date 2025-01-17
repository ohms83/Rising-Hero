using Character;
using Character.Data;
using Event;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay.System.LevelUp
{
    [RequireComponent(typeof(GameCharacter))]
    public class LevelUpSystem : MonoBehaviour
    {
        [SerializeField] private LevelUpData m_levelUpData;
        [Tooltip("An event-bus that will inform the system about the enemy's death.")]
        [SerializeField] private CharacterEvent m_enemyDeathEvent;
        public UnityEvent<GameCharacter, LevelUpData> levelUpEvent;

        private GameCharacter m_player;

        private void Awake()
        {
            m_player = GetComponent<GameCharacter>();
            m_enemyDeathEvent.onEventRaised += OnEnemyDeath;
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
            while (m_levelUpData.IsLevelUp)
            {
                levelUpEvent?.Invoke(m_player, m_levelUpData);

                m_levelUpData.xp -= m_levelUpData.ToNextLevel;
                ++m_levelUpData.level;
            }
        }
    }
}

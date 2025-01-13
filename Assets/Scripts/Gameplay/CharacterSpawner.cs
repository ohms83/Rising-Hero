using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Character;
using Character.Controller;
using ScriptableObjects.Character;
using ScriptableObjects.Event;
using ScriptableObjects.Gameplay;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace Gameplay
{
    public class CharacterSpawner : MonoBehaviour
    {
        [SerializeField] private SpawnerData spawnerData;
        [SerializeField] private CharacterEvent playerSpawnedEvent;
        [SerializeField] private CharacterEvent playerDeathEvent;

        private readonly List<GameCharacter> m_playerCharacters = new ();
        private readonly List<GameCharacter> m_spawnedEnemies = new ();
        private IEnumerator m_spawnCoroutine;

        private void Awake()
        {
            m_spawnCoroutine = SpawnLoop();
        }

        private void Start()
        {
            Assert.IsNotNull(spawnerData);
            
            // Spawning players
            foreach (var playerCharacter in
                     spawnerData.playerCharacters.Select(characterData => Instantiate(characterData.prefab)))
            {
                m_playerCharacters.Add(playerCharacter);
                
                playerSpawnedEvent.onEventRaised?.Invoke(playerCharacter);
            }

            if (playerDeathEvent != null)
                playerDeathEvent.onEventRaised += OnPlayerDeath;
        }

        private void OnEnable()
        {
            StartCoroutine(m_spawnCoroutine);
        }

        private void OnDisable()
        {
            StopCoroutine(m_spawnCoroutine);
        }

        private void OnPlayerDeath(GameCharacter player)
        {
            enabled = false;
        }

        private IEnumerator SpawnLoop()
        {
            if (spawnerData.enemyCharacters.Count == 0)
            {
                yield break;
            }
            
            while (true)
            {
                yield return new WaitForSeconds(spawnerData.spawnInterval);
                
                if (m_spawnedEnemies.Count >= spawnerData.maxEnemySpawnCount)
                    continue;

                var radian = Random.Range(0, 2 * Mathf.PI);
                var x = Mathf.Cos(radian) * spawnerData.enemySpawnRadius;
                var y = Mathf.Sin(radian) * spawnerData.enemySpawnRadius;
                var playerPos = GetPlayerPosition();
                
                // TODO: Spawn various enemy types
                var enemyData = (EnemyCharacterData)spawnerData.enemyCharacters[0];
                Assert.IsNotNull(enemyData, $"{spawnerData.enemyCharacters[0]} is not a EnemyCharacterData type.");
                
                var enemy = Instantiate(
                    enemyData.prefab,
                    playerPos + new Vector3(x, y, 0),
                    Quaternion.identity
                );
                enemy.onCharacterDestroyed.AddListener(character => m_spawnedEnemies.Remove(character));
                
                // Init AI
                var enemyAI = (AIController)enemy.Controller;
                Assert.IsNotNull(enemyAI);
                enemyAI.PlayerCharacter = m_playerCharacters[0];
                
                m_spawnedEnemies.Add(enemy);
            }
        }

        private Vector3 GetPlayerPosition()
        {
            return m_playerCharacters.Count == 0 ? Vector3.zero : m_playerCharacters[0].transform.position;
        }
    }
}

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
            SpawnPlayers();
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

        private void SpawnPlayers()
        {
            foreach (var playerCharacter in
                     spawnerData.playerCharacters.Select(characterData => Instantiate(characterData.prefab)))
            {
                playerCharacter.SetAutoCast(spawnerData.playerAutoCastSkills);
                playerCharacter.onCharacterDeath.AddListener(OnPlayerDeath);
                playerSpawnedEvent.onEventRaised?.Invoke(playerCharacter);
                m_playerCharacters.Add(playerCharacter);
            }

            if (m_playerCharacters.Count > 0)
                AssignController(spawnerData.playerControllerPrefab, m_playerCharacters[0]);
        }

        private static ControllerBase AssignController(ControllerBase prefab, GameCharacter character)
        {
            if (prefab == null)
                return null;
            var controller = Instantiate(prefab, character.transform, true);
            controller.ControlledCharacter = character;
            return controller;
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
                var aiController = (AIController)AssignController(enemyData.aiControllerPrefab, enemy);
                if (aiController != null)
                {
                    // TODO: Handle multiple players
                    aiController.PlayerCharacter = m_playerCharacters[0];
                }

                m_spawnedEnemies.Add(enemy);
            }
        }

        private Vector3 GetPlayerPosition()
        {
            return m_playerCharacters.Count == 0 ? Vector3.zero : m_playerCharacters[0].transform.position;
        }
    }
}

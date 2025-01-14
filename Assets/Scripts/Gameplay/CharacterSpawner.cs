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
        [SerializeField] private SpawnerData m_spawnerData;
        [SerializeField] private CharacterEvent m_playerSpawnedEvent;

        private readonly List<GameCharacter> m_playerCharacters = new ();
        private readonly List<GameCharacter> m_spawnedEnemies = new ();
        private IEnumerator m_spawnCoroutine;
        private readonly List<IEnumerator> m_spawnCoroutines = new ();

        private void Awake()
        {
            foreach (var spawnData in m_spawnerData.enemyData)
            {
                m_spawnCoroutines.Add(SpawnLoop(spawnData));
            }
        }

        private void Start()
        {
            Assert.IsNotNull(m_spawnerData);
            SpawnPlayers();
        }

        private void OnEnable()
        {
            foreach (var coroutine in m_spawnCoroutines)
            {
                StartCoroutine(coroutine);
            }
        }

        private void OnDisable()
        {
            foreach (var coroutine in m_spawnCoroutines)
            {
                StopCoroutine(coroutine);
            }
        }

        private void OnPlayerDeath(GameCharacter player)
        {
            enabled = false;
        }

        private void SpawnPlayers()
        {
            foreach (var playerCharacter in
                     m_spawnerData.playerCharacters.Select(characterData => Instantiate(characterData.prefab)))
            {
                playerCharacter.SetAutoCast(m_spawnerData.playerAutoCastSkills);
                playerCharacter.onCharacterDeath.AddListener(OnPlayerDeath);
                m_playerSpawnedEvent.onEventRaised?.Invoke(playerCharacter);
                m_playerCharacters.Add(playerCharacter);
            }

            if (m_playerCharacters.Count > 0)
                AssignController(m_spawnerData.playerControllerPrefab, m_playerCharacters[0]);
        }

        private static ControllerBase AssignController(ControllerBase prefab, GameCharacter character)
        {
            if (prefab == null)
                return null;
            var controller = Instantiate(prefab, character.transform, true);
            controller.ControlledCharacter = character;
            return controller;
        }

        private IEnumerator SpawnLoop(EnemySpawnData spawnData)
        {
            if (spawnData.enemyData.prefab == null)
            {
                yield break;
            }
            
            while (true)
            {
                yield return new WaitForSeconds(spawnData.spawnInterval);
                
                if (m_spawnedEnemies.Count >= spawnData.maxEnemySpawnCount)
                    continue;

                var radian = Random.Range(0, 2 * Mathf.PI);
                var x = Mathf.Cos(radian) * spawnData.enemySpawnRadius;
                var y = Mathf.Sin(radian) * spawnData.enemySpawnRadius;
                var playerPos = GetPlayerPosition();
                
                var enemyData = (EnemyCharacterData)spawnData.enemyData;
                Assert.IsNotNull(enemyData, $"{spawnData.enemyData} is not a EnemyCharacterData type.");
                
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

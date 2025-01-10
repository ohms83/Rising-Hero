using System.Collections;
using System.Collections.Generic;
using Character;
using Character.Controller.AI;
using ScriptableObjects.Event;
using ScriptableObjects.Gameplay;
using UnityEngine;
using UnityEngine.Assertions;

namespace Gameplay
{
    public class CharacterSpawner : MonoBehaviour
    {
        [SerializeField] private SpawnerData spawnerData;
        [SerializeField] private CharacterSpawnedEvent playerSpawnedEvent;

        private readonly List<GameCharacter> m_playerCharacters = new ();
        private readonly List<GameCharacter> m_spawnedEnemies = new ();
        private void Start()
        {
            Assert.IsNotNull(spawnerData);
            
            // Spawning players
            foreach (var characterData in spawnerData.playerCharacters)
            {
                var playerCharacter = Instantiate(characterData.prefab);
                m_playerCharacters.Add(playerCharacter);
                
                playerSpawnedEvent.onEventRaised?.Invoke(new SpawnedCharacterEventData
                {
                    characterData = characterData,
                    spawnedCharacter = playerCharacter
                });
            }
            
            StartCoroutine(SpawnLoop());
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
                var enemy = Instantiate(
                    spawnerData.enemyCharacters[0].prefab,
                    playerPos + new Vector3(x, y, 0),
                    Quaternion.identity
                );
                enemy.onCharacterDestroyed += character => m_spawnedEnemies.Remove(character);
                
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

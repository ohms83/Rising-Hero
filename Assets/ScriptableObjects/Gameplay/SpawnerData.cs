using System.Collections.Generic;
using Character.Controller;
using ScriptableObjects.Character;
using Unity.Cinemachine;
using UnityEngine;

namespace ScriptableObjects.Gameplay
{
    [CreateAssetMenu(fileName = "SpawnerData", menuName = "Scriptable Objects/SpawnerData")]
    public class SpawnerData : ScriptableObject
    {
        [Header("Player")]
        public List<GameCharacterData> playerCharacters;
        [Tooltip("If set, player's AutoCastAllSkills flag will be raised.")]
        public bool playerAutoCastSkills;
        public ControllerBase playerControllerPrefab;
        
        [Header("Enemy")]
        public List<GameCharacterData> enemyCharacters;
        /// <summary>
        /// Enemy spawning interval in seconds
        /// </summary>
        public float spawnInterval = 2f;
        public int maxEnemySpawnCount = 50;
        public float enemySpawnRadius = 10f;
    }
}

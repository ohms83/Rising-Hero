using System;
using System.Collections.Generic;
using Character.Controller;
using ScriptableObjects.Character;
using UnityEngine;

namespace ScriptableObjects.Gameplay
{
    [Serializable]
    public struct EnemySpawnData
    {
        public GameCharacterData enemyData;
        [Tooltip("Enemy spawning interval in seconds")]
        public float spawnInterval;
        public int maxEnemySpawnCount;
        public float enemySpawnRadius;
    }
    [CreateAssetMenu(fileName = "SpawnerData", menuName = "Scriptable Objects/SpawnerData")]
    public class SpawnerData : ScriptableObject
    {
        [Header("Player")]
        public List<GameCharacterData> playerCharacters;
        [Tooltip("If set, player's AutoCastAllSkills flag will be raised.")]
        public bool playerAutoCastSkills;
        public ControllerBase playerControllerPrefab;
        
        [Header("Enemy")]
        public List<EnemySpawnData> enemyData;
    }
}

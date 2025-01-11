using System.Collections.Generic;
using ScriptableObjects.Character;
using Unity.Cinemachine;
using UnityEngine;

namespace ScriptableObjects.Gameplay
{
    [CreateAssetMenu(fileName = "SpawnerData", menuName = "Scriptable Objects/SpawnerData")]
    public class SpawnerData : ScriptableObject
    {
        public List<GameCharacterData> playerCharacters;
        public List<GameCharacterData> enemyCharacters;
        /// <summary>
        /// Enemy spawning interval in seconds
        /// </summary>
        public float spawnInterval = 2f;
        public int maxEnemySpawnCount = 50;
        public float enemySpawnRadius = 10f;
    }
}

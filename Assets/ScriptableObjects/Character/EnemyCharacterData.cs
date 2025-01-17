using System.Collections.Generic;
using Character.Controller;
using Pattern;
using ScriptableObjects.Character.Controller.AIState;
using UnityEngine;

namespace ScriptableObjects.Character
{
    [CreateAssetMenu(fileName = "EnemyCharacterData", menuName = "Scriptable Objects/Character/EnemyCharacterData", order = 2)]
    public class EnemyCharacterData : GameCharacterData
    {
        public AIController aiControllerPrefab;
        [Tooltip("How many experience points the enemy will give out once defeated.")]
        public float xp;
    }
}
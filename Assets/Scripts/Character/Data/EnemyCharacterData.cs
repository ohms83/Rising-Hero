using Character.Controller;
using UnityEngine;

namespace Character.Data
{
    [CreateAssetMenu(fileName = "EnemyCharacterData", menuName = "Scriptable Objects/Character/EnemyCharacterData", order = 2)]
    public class EnemyCharacterData : GameCharacterData
    {
        public AIController aiControllerPrefab;
        [Tooltip("How many experience points the enemy will give out once defeated.")]
        public float xp;
    }
}
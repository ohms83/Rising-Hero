using System.Collections.Generic;
using Pattern;
using ScriptableObjects.Character.Controller.AIState;
using UnityEngine;

namespace ScriptableObjects.Character
{
    [CreateAssetMenu(fileName = "EnemyCharacterData", menuName = "Scriptable Objects/Character/EnemyCharacterData", order = 2)]
    public class EnemyCharacterData : GameCharacterData
    {
        [Tooltip("A list of concrete AI states that will be registered to the AIController.")]
        public List<AIState> aiStates;
    }
}